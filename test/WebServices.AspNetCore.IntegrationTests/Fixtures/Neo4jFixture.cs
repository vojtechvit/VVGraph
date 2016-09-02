using Neo4jClient;
using System;

namespace WebServices.AspNetCore.IntegrationTests.Fixtures
{
    public sealed class Neo4jFixture : IDisposable
    {
        public Neo4jFixture()
        {
            Client = new GraphClient(new Uri("http://test"));
            Client.Connect();
        }

        public GraphClient Client { get; }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}