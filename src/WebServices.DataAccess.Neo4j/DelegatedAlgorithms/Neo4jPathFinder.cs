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

            var result = (await client
                .Cypher
                .Match("(g:Graph)<-[:PART_OF]-(sn:Node)",
                    "p=shortestPath((sn)-[:ADJACENT_TO*]-(en:Node))")
                .Where((Model.Graph g) => g.Name == graph.Name)
                .AndWhere((Model.Node sn) => sn.Id == startNode.Id)
                .AndWhere((Model.Node en) => en.Id == endNode.Id)
                .Return(() => new
                {
                    NodeIds = Return.As<IEnumerable<int>>("EXTRACT(n in nodes(p) | n.id)")
                })
                .Limit(1)
                .ResultsAsync)
                .FirstOrDefault();

            if (result == null)
            {
                return null;
            }

            return pathFactory.Create(result.NodeIds.Select(nid => graph.Nodes[nid]));
        }
    }
}