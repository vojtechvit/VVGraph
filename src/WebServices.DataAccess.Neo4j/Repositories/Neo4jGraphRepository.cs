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

        public async Task<Graph> GetAsync(string name)
        {
            var graphAndNodes = (await client
                .Cypher
                .Match("(graph:Graph)<-[:PART_OF]-(node:Node)")
                .Where((Graph graph) => graph.Name == name)
                .Return((graph, node, edge) => new
                {
                    Graph = graph.As<Model.Graph>(),
                    Nodes = node.CollectAs<Model.Node>()
                })
                .Limit(1)
                .ResultsAsync)
                .FirstOrDefault();

            if (graphAndNodes == null)
            {
                return null;
            }

            var edges = await client
                .Cypher
                .Match("(graph:Graph)<-[:PART_OF]-(n1:Node)-[:ADJACENT_TO]->(n2:Node)")
                .Where((Graph graph) => graph.Name == name)
                .Return((n1, n2) => new Model.Edge
                {
                    StartNodeId = n1.As<Model.Node>().Id,
                    EndNodeId = n2.As<Model.Node>().Id
                })
                .ResultsAsync;

            return graphMapper.Map(graphAndNodes.Graph, graphAndNodes.Nodes, edges);
        }

        public async Task CreateAsync(Graph graph)
        {
            var dbGraph = graphMapper.MapGraph(graph);
            var dbNodes = graphMapper.MapNodes(graph);
            var dbEdges = graphMapper.MapEdges(graph);

            //// Pending issue with authorization inside transaction - a bug in Neo4jClient.
            //// A workaround exists, but not applied.
            //// see http://stackoverflow.com/questions/31519633/neo4jclient-transactions-auth-error-on-transaction-completion
            //// using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            //// {
            // Create graph and its nodes
            await client
                .Cypher
                .WithParam("properties", dbGraph)
                .Create("(g:Graph {properties})")
                .With("g")
                .Unwind(dbNodes, "node")
                .Create("(n:Node)-[:PART_OF]->(g)")
                .Set("n = node")
                .ExecuteWithoutResultsAsync();

            // Add edges to the nodes of the graph
            await client
                .Cypher
                .Unwind(dbEdges, "edge")
                .Match("(g:Graph)<-[:PART_OF]-(node1:Node)",
                    "(g:Graph)<-[:PART_OF]-(node2:Node)")
                .Where((Model.Graph g) => g.Name == dbGraph.Name)
                .AndWhere((Model.Node node1, Model.Edge edge) => node1.Id == edge.StartNodeId)
                .AndWhere((Model.Node node2, Model.Edge edge) => node2.Id == edge.EndNodeId)
                .CreateUnique("(node1)-[:ADJACENT_TO]->(node2)")
                .ExecuteWithoutResultsAsync();

            ////     scope.Complete();
            //// }
        }

        public async Task DeleteAsync(string name)
        {
            await client
                .Cypher
                .OptionalMatch("(graph:Graph)<-[*0..1]-(x)")
                .Where((Graph graph) => graph.Name == name)
                .DetachDelete("x")
                .ExecuteWithoutResultsAsync();
        }
    }
}