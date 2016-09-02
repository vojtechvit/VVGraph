using Microsoft.AspNetCore.TestHost;
using System.Net;
using System.Threading.Tasks;
using WebServices.AspNetCore.IntegrationTests.Fixtures;
using Xunit;

namespace WebServices.AspNetCore.IntegrationTests.Graphs
{
    public sealed class Get : IClassFixture<TestServerFactory>
    {
        private readonly TestServerBuilder serverBuilder;

        public Get(TestServerFactory serverFactory)
        {
            this.serverBuilder = serverFactory.Create();
        }

        private string RequestGraphName { get; set; } = "test1";

        private string RequestPath => string.Format("/api/v1/graphs/{0}", RequestGraphName);

        public RequestBuilder CreateBaseValidRequest()
            => new RequestBuilder(serverBuilder.Build(), RequestPath);

        [Fact]
        public async Task GraphExists_Ok()
        {
            //// Arrange
            var request = CreateBaseValidRequest();

            //// Act
            var response = await request.GetAsync();

            //// Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}