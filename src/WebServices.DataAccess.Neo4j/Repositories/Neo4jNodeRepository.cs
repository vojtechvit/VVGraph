using WebServices.DataAccess.Neo4j.Contracts;
using Domain.Factories.Contracts;
using Domain.Model;
using Domain.Repositories.Contracts;
using System;
using System.Collections.Generic;

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

        public IReadOnlyCollection<Node> GetAllNodesForGraph(string graphName)
        {
            throw new NotImplementedException();
        }

        public void CreateAll(IEnumerable<Node> node)
        {
            throw new NotImplementedException();
        }

        public void DeleteAllForGraph(string graphName)
        {
            throw new NotImplementedException();
        }
    }
}