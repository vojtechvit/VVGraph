using WebServices.DataAccess.Neo4j.Contracts;
using Domain.Factories.Contracts;
using Domain.Model;
using Domain.Repositories.Contracts;
using System;

namespace WebServices.DataAccess.Neo4j.Repositories
{
    public class Neo4jGraphRepository : IGraphRepository
    {
        private readonly INeo4jDriver driver;

        private readonly IGraphFactory graphFactory;

        public Neo4jGraphRepository(
            INeo4jDriver driver,
            IGraphFactory graphFactory)
        {
            if (driver == null)
                throw new ArgumentNullException(nameof(driver));

            if (graphFactory == null)
                throw new ArgumentNullException(nameof(graphFactory));

            this.driver = driver;
            this.graphFactory = graphFactory;
        }

        public bool Exists(string name)
        {
            throw new NotImplementedException();
        }

        public Graph Get(string name)
        {
            throw new NotImplementedException();
        }

        public void Create(Graph graph)
        {
            throw new NotImplementedException();
        }

        public void Delete(string name)
        {
            throw new NotImplementedException();
        }
    }
}