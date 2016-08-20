using Domain.Factories.Contracts;
using Domain.Model;
using Domain.Repositories.Contracts;
using System;
using System.Threading.Tasks;
using WebServices.DataAccess.Neo4j.Contracts;

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

        public Task<bool> ExistsAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task<Graph> GetAsync(string name)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(Graph graph)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string name)
        {
            throw new NotImplementedException();
        }
    }
}