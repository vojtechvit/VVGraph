using DataAccess.Neo4j.Contracts;
using Domain.Algorithms.Contracts;
using Domain.Factories.Contracts;
using Domain.Model;
using System;
using System.Collections.Generic;

namespace DataAccess.Neo4j.DelegatedAlgorithms
{
    public sealed class EdgeEnumerator : IEdgeEnumerator
    {
        private readonly INeo4jDriver driver;
        private readonly IEdgeFactory edgeFactory;

        public EdgeEnumerator(
            INeo4jDriver driver,
            IEdgeFactory edgeFactory)
        {
            if (driver == null)
                throw new ArgumentNullException(nameof(driver));

            if (edgeFactory == null)
                throw new ArgumentNullException(nameof(edgeFactory));

            this.driver = driver;
            this.edgeFactory = edgeFactory;
        }

        public IReadOnlyCollection<Edge> GetAllEdges(string graphName)
        {
            throw new NotImplementedException();
        }
    }
}