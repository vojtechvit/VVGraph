using Microsoft.AspNetCore.TestHost;
using WebServices.AspNetCore.IntegrationTests.Fixtures;

namespace WebServices.AspNetCore.IntegrationTests.Graphs
{
    public sealed class GetRequestBuilder : RequestBuilderBase
    {
        public GetRequestBuilder(TestServerBuilder serverBuilder) : base(serverBuilder)
        {
        }

        public string GraphName { get; set; } = "test1";

        public string Path => string.Format("/api/v1/graphs/{0}", GraphName);

        public RequestBuilder Build()
            => new RequestBuilder(ServerBuilder.Build(), Path)
            .Method("GET");
    }
}