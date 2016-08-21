using Domain.Algorithms.Contracts;
using Domain.Factories.Contracts;
using Domain.Model;
using Neo4jClient;
using Neo4jClient.Cypher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServices.DataAccess.Neo4j.DelegatedAlgorithms
{
    public sealed class Neo4jPathFinder : IPathFinder
    {
        private readonly IGraphClient client;
        private readonly IPathFactory pathFactory;

        public Neo4jPathFinder(
            IGraphClient client,
            IPathFactory pathFactory)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            if (pathFactory == null)
                throw new ArgumentNullException(nameof(pathFactory));

            this.client = client;
            this.pathFactory = pathFactory;
        }

        public async Task<Path> FindShortestPathAsync(Graph graph, Domain.Model.Node startNode, Domain.Model.Node endNode)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (startNode == null)
                throw new ArgumentNullException(nameof(startNode));

            if (endNode == null)
                throw new ArgumentNullException(nameof(endNode));

            var nodeIds = new List<int>();

            var query = client
                .Cypher
                .Match("(g:Graph)<-[:PART_OF]-(sn:Node),(g:Graph)<-[:PART_OF]-(en:Node),p=shortestPath((sn)-[*]-(oliver))")
                .Where((Model.Graph g) => g.Name == g.Name)
                .AndWhere((Model.Node sn) => sn.Id == startNode.Id)
                .AndWhere((Model.Node en) => en.Id == endNode.Id)
                .AndWhere("ALL(r IN rels(p) WHERE type(r)=\"ADJACENT_TO\")")
                .Return(() => new
                {
                    NodeIds = Return.As<IEnumerable<int>>("EXTRACT(n in nodes(p) | n.id)")
                });

            var results = await query.ResultsAsync;

            var result = results.FirstOrDefault();

            if (result == null)
            {
                return null;
            }

            return pathFactory.Create(result.NodeIds.Select(nid => graph.Nodes[nid]));
        }
    }
}