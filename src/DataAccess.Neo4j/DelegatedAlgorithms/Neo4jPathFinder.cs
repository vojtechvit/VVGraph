using Domain.Algorithms.Contracts;
using Domain.Factories.Contracts;
using Domain.Model;
using System;
using System.Collections.Generic;
using DataAccess.Neo4j.Contracts;

namespace DataAccess.Neo4j.DelegatedAlgorithms
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

        public Path GetShortestPath(string graphName, int startNodeId, int endNodeId)
        {
            if (graphName == null)
                throw new ArgumentNullException(nameof(graphName));

            var nodeIds = new List<int>();

            using (var session = driver.Session())
            {
            }

            return pathFactory.Create(graphName, nodeIds);
        }
    }
}