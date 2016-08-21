using Domain.Factories.Contracts;
using Domain.Model;
using Domain.Repositories.Contracts;
using Neo4jClient;
using Neo4jClient.Transactions;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using WebServices.DataAccess.Neo4j.Mappers.Contracts;

namespace WebServices.DataAccess.Neo4j.Repositories
{
    public class Neo4jGraphRepository : IGraphRepository
    {
        private readonly IGraphClient client;
        private readonly IGraphFactory graphFactory;
        private readonly INeo4jGraphMapper graphMapper;

        public Neo4jGraphRepository(
            IGraphClient client,
            IGraphFactory graphFactory,
            INeo4jGraphMapper graphMapper)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            if (graphFactory == null)
                throw new ArgumentNullException(nameof(graphFactory));

            if (graphMapper == null)
                throw new ArgumentNullException(nameof(graphMapper));

            this.client = client;
            this.graphFactory = graphFactory;
            this.graphMapper = graphMapper;
        }

        public async Task<bool> ExistsAsync(string name)
        {
            var query = client
                .Cypher
                .Match("(graph:Graph)")
                .Where((Graph graph) => graph.Name == name)
                .Return<int>("1");

            var results = await query.ResultsAsync;

            return results.Any();
        }

        public async Task<Graph> GetAsync(string name)
        {
            var query = client
             .Cypher
             .Match("(:Graph)<-[:PART_OF]-(node:Node)-[edge:ADJACENT_TO]->(:Node)")
             .Where((Graph graph) => graph.Name == name)
             .Return((graph, node, edge) => new
             {
                 Graph = graph.As<Model.Graph>(),
                 Nodes = node.CollectAs<Model.Node>(),
                 Edges = edge.CollectAs<Model.Edge>()
             });

            var results = await query.ResultsAsync;

            var firstResult = results.FirstOrDefault();

            if (firstResult == null)
            {
                return null;
            }

            return graphMapper.Map(firstResult.Graph, firstResult.Nodes, firstResult.Edges);
        }

        public async Task CreateAsync(Graph graph)
        {
            var dbGraph = graphMapper.MapGraph(graph);
            var dbNodes = graphMapper.MapNodes(graph);

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await client
                    .Cypher
                    .Create("(graph:Graph {properties})")
                    .WithParam("properties", dbGraph)
                    .ExecuteWithoutResultsAsync();

                await client
                    .Cypher
                    .Match("(graph:Graph)")
                    .Where((Graph g) => g.Name == dbGraph.Name)
                    .Unwind(dbNodes, "properties")
                    .CreateUnique("(node:Node {properties})-[:PART_OF]->(graph:Graph)")
                    .ExecuteWithoutResultsAsync();

                foreach (var edge in graph.Edges)
                {
                    await client
                        .Cypher
                        .Match("(node1:Node)", "(node2:Node)")
                        .Where((Node node1) => node1.Id == edge.StartNode.Id)
                        .AndWhere((Node node2) => node2.Id == edge.EndNode.Id)
                        .CreateUnique("node1-[:ADJACENT_TO]->node1")
                        .ExecuteWithoutResultsAsync();
                }

                scope.Complete();
            }
        }

        public async Task DeleteAsync(string name)
        {
            await client
                .Cypher
                .OptionalMatch("(graph:Graph)<-[:ADJACENT_TO]-(node:Node)")
                .Where((Graph graph) => graph.Name == name)
                .DetachDelete("graph, node")
                .ExecuteWithoutResultsAsync();
        }
    }
}