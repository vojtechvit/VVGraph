using Domain.Algorithms.Contracts;
using Domain.Factories.Contracts;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebServices.DataAccess.Neo4j.Contracts;

namespace WebServices.DataAccess.Neo4j.DelegatedAlgorithms
{
    public sealed class Neo4jPathFinder : IPathFinder
    {
        private readonly INeo4jDriver driver;
        private readonly IPathFactory pathFactory;

        public Neo4jPathFinder(
            INeo4jDriver driver,
            IPathFactory pathFactory)
        {
            if (driver == null)
                throw new ArgumentNullException(nameof(driver));

            if (pathFactory == null)
                throw new ArgumentNullException(nameof(pathFactory));

            this.driver = driver;
            this.pathFactory = pathFactory;
        }

        public async Task<Path> FindShortestPathAsync(Graph graph, Node startNode, Node endNode)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (startNode == null)
                throw new ArgumentNullException(nameof(startNode));

            if (endNode == null)
                throw new ArgumentNullException(nameof(endNode));

            var nodeIds = new List<int>();

            using (var session = driver.Session())
            {
            }

            return pathFactory.Create(graph.Nodes.Values);
        }
    }
}