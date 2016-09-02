using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using WebServices.AspNetCore.IntegrationTests.Fixtures;
using WebServices.AspNetCore.IntegrationTests.Graphs.Model;

namespace WebServices.AspNetCore.IntegrationTests.Graphs
{
    public sealed class PutRequestBuilder : RequestBuilderBase
    {
        private string requestPayloadRaw;

        public PutRequestBuilder(TestServerBuilder serverBuilder) : base(serverBuilder)
        {
        }

        public string GraphName { get; set; } = Guid.NewGuid().ToString("N");

        public string Path => string.Format("/api/v1/graphs/{0}", GraphName);

        public Graph PayloadObject { get; set; }

        public Encoding Encoding { get; set; } = Encoding.UTF8;

        public string ContentType { get; set; } = "application/json";

        public string Payload
        {
            get { return requestPayloadRaw ?? JsonConvert.SerializeObject(PayloadObject); }
            set { requestPayloadRaw = value; }
        }

        public RequestBuilder Build()
            => new RequestBuilder(ServerBuilder.Build(), Path)
            .Method("PUT")
            .And(m => m.Content = new StringContent(Payload, Encoding, ContentType));
    }
}