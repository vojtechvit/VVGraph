using Domain.Algorithms.Contracts;
using Domain.Factories.Contracts;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebServices.DataAccess.Neo4j.Contracts;

namespace WebServices.DataAccess.Neo4j.DelegatedAlgorithms
{
    public sealed class Neo4jEdgeEnumerator : IEdgeEnumerator
    {
        private readonly INeo4jDriver driver;
        private readonly IEdgeFactory edgeFactory;

        public Neo4jEdgeEnumerator(
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

        public Task<IReadOnlyCollection<Edge>> GetAllEdgesAsync(string graphName)
        {
            throw new NotImplementedException();
        }
    }
}