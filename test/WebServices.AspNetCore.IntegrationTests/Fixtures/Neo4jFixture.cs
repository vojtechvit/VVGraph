using Neo4jClient;
using System;

namespace WebServices.AspNetCore.IntegrationTests.Fixtures
{
    public sealed class Neo4jFixture : IDisposable
    {
        public Neo4jFixture()
        {
            Client = new GraphClient(new Uri("http://neo4j:pass123@localhost:7474/db/data"));
            Client.Connect();
        }

        public GraphClient Client { get; }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}