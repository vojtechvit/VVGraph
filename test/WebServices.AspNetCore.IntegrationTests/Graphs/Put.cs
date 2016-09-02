using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebServices.AspNetCore.IntegrationTests.Fixtures;
using WebServices.AspNetCore.IntegrationTests.Graphs.Model;
using WebServices.AspNetCore.IntegrationTests.Graphs.SampleData;
using Xunit;

namespace WebServices.AspNetCore.IntegrationTests.Graphs
{
    public sealed class Put
    {
        private readonly TestServerBuilder serverBuilder;

        private string requestPayloadRaw;

        public Put(TestServerFactory serverFactory)
        {
            this.serverBuilder = serverFactory.Create();
        }

        private string RequestGraphName { get; set; } = Guid.NewGuid().ToString("N");

        private string RequestPath => string.Format("/api/v1/graphs/{0}", RequestGraphName);

        private Graph RequestPayloadObject { get; set; }

        private Encoding RequestEncoding { get; set; } = Encoding.UTF8;

        private string RequestContentType { get; set; } = "application/json";

        public string RequestPayload
        {
            get { return requestPayloadRaw ?? JsonConvert.SerializeObject(RequestPayloadObject); }
            set { requestPayloadRaw = value; }
        }

        public RequestBuilder CreateBaseValidRequest()
            => new RequestBuilder(serverBuilder.Build(), RequestPath)
            .And(m => m.Content = new StringContent(RequestPayload, RequestEncoding, RequestContentType));

        [Theory]
        [AllValidGraphs]
        public async Task ValidNonexistentGraphs_NoContent(Graph graph)
        {
            //// Arrange
            RequestPayloadObject = graph;
            var request = CreateBaseValidRequest();

            //// Act
            var response = await request.SendAsync("PUT");

            //// Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}