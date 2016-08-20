using Domain.Factories.Contracts;
using Domain.Model;
using Domain.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebServices.DataAccess.Neo4j.Contracts;

namespace WebServices.DataAccess.Neo4j.Repositories
{
    public sealed class Neo4jNodeRepository : INodeRepository
    {
        private readonly INeo4jDriver driver;

        private readonly INodeFactory nodeFactory;

        public Neo4jNodeRepository(
            INeo4jDriver driver,
            INodeFactory nodeFactory)
        {
            if (driver == null)
                throw new ArgumentNullException(nameof(driver));

            if (nodeFactory == null)
                throw new ArgumentNullException(nameof(nodeFactory));

            this.driver = driver;
            this.nodeFactory = nodeFactory;
        }

        public Task<IReadOnlyCollection<Node>> GetAllNodesForGraphAsync(string graphName)
        {
            throw new NotImplementedException();
        }

        public Task CreateAllAsync(IEnumerable<Node> node)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAllForGraphAsync(string graphName)
        {
            throw new NotImplementedException();
        }
    }
}