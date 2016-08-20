using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebServices.ApiModel;
using WebServices.AspNetCore.Proxy.Contracts;

namespace WebServices.AspNetCore.Proxy
{
    public sealed class VVGraphClient : IVVGraphClient, IDisposable
    {
        private const string JsonMimeType = "application/json";

        private readonly HttpClient httpClient;

        private readonly VVGraphClientConfiguration configuration;

        private bool disposed;

        public VVGraphClient(
            VVGraphClientConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            httpClient = new HttpClient();
            this.configuration = configuration;
        }

        public async Task PutGraphAsync(Graph graph, CancellationToken cancellationToken)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            try
            {
                var graphUrl = GetGraphUrl(graph.Name);

                var body = JsonConvert.SerializeObject(graph);
                var httpContent = new StringContent(body, Encoding.UTF8, JsonMimeType);

                await httpClient.PutAsync(graphUrl, httpContent, cancellationToken);
            }
            catch (Exception exception)
            {
                var exceptionWrap = new VVGraphClientException("A graph could not be created/replaced.", exception);
                exceptionWrap.Data["GraphName"] = graph.Name;
                throw exceptionWrap;
            }
        }

        public void Dispose()
        {
            if (!disposed)
            {
                httpClient.Dispose();
                disposed = true;
            }
        }

        private Uri GetGraphUrl(string graphName)
            => new Uri(configuration.BaseUrl, $"graphs/{graphName}");
    }
}