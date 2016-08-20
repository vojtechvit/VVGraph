using Domain.Model;
using Domain.Repositories.Contracts;
using System;

namespace DataAccess.Neo4j.Repositories
{
    public class Neo4jGraphRepository : IGraphRepository
    {
        public void CreateOrReplace(Graph graph)
        {
            throw new NotImplementedException();
        }

        public bool Exists()
        {
            throw new NotImplementedException();
        }

        public Graph Get(string name)
        {
            throw new NotImplementedException();
        }
    }
}