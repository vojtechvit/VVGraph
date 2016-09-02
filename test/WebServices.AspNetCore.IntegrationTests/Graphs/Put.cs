using Neo4jClient;
using Neo4jClient.Transactions;
using System;
using System.Net;
using System.Threading.Tasks;
using WebServices.AspNetCore.IntegrationTests.Fixtures;
using WebServices.AspNetCore.IntegrationTests.Graphs.Model;
using WebServices.AspNetCore.IntegrationTests.Graphs.SampleData;
using Xunit;

namespace WebServices.AspNetCore.IntegrationTests.Graphs
{
    public sealed class Put : IDisposable, IClassFixture<TestServerFactory>, IClassFixture<Neo4jFixture>
    {
        private readonly PutRequestBuilder requestBuilder;

        private readonly ITransactionalGraphClient graphClient;

        public Put(TestServerFactory serverFactory, Neo4jFixture neo4j)
        {
            var serverBuilder = serverFactory.Create();
            requestBuilder = new PutRequestBuilder(serverBuilder);
            graphClient = neo4j.Client;

            serverBuilder.ConfigureServices(services =>
            {
                services.ReplaceWithSingleton<IGraphClient>(neo4j.Client);
            });

            graphClient.BeginTransaction();
        }

        public void Dispose()
        {
            graphClient.EndTransaction();
        }

        [Theory]
        [AllValidGraphs]
        public async Task ValidNonexistentGraphs_NoContent(Graph graph)
        {
            //// Arrange
            requestBuilder.PayloadObject = graph;
            var request = requestBuilder.Build();

            //// Act
            var response = await request.SendAsync();

            //// Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}