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
    public sealed class Get : IDisposable, IClassFixture<TestServerFactory>, IClassFixture<Neo4jFixture>
    {
        private readonly TestServerBuilder serverBuilder;

        private readonly GetRequestBuilder requestBuilder;

        private readonly ITransactionalGraphClient graphClient;

        public Get(TestServerFactory serverFactory, Neo4jFixture neo4j)
        {
            serverBuilder = serverFactory.Create();
            requestBuilder = new GetRequestBuilder(serverBuilder);
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
        public async Task GraphExists_Ok(Graph graph)
        {
            //// Arrange
            await CreateGraphInNeo4jAsync(graph);
            var request = requestBuilder.Build();

            //// Act
            var response = await request.SendAsync();

            //// Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseObject = await response.Content.ReadAsJsonAsync<Graph>();
            GraphAssert.Equal(graph, responseObject);
        }

        private async Task CreateGraphInNeo4jAsync(Graph graph)
        {
            var putRequestBuilder = new PutRequestBuilder(serverBuilder)
            {
                GraphName = graph.Name,
                PayloadObject = graph
            };

            await putRequestBuilder.Build().SendAsync();
        }
    }
}