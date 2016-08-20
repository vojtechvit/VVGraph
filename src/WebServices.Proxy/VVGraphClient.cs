using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebServices.ApiModel;
using WebServices.Proxy.Contracts;

namespace WebServices.Proxy
{
    public sealed class VVGraphClient : IVVGraphClient, IDisposable
    {
        private const string JsonMimeType = "application/json";

        private readonly VVGraphClientConfiguration configuration;
        private readonly IHttpClient httpClient;
        private readonly IJsonSerializer jsonSerializer;

        private bool disposed;

        public VVGraphClient(
            VVGraphClientConfiguration configuration,
            IHttpClient httpClient,
            IJsonSerializer jsonSerializer)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            if (jsonSerializer == null)
                throw new ArgumentNullException(nameof(jsonSerializer));

            this.httpClient = httpClient;
            this.configuration = configuration;
            this.jsonSerializer = jsonSerializer;
        }

        public async Task PutGraphAsync(Graph graph, CancellationToken cancellationToken)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            try
            {
                var graphUrl = GetGraphUrl(graph.Name);

                var body = jsonSerializer.Serialize(graph);

                await httpClient.PutAsync(graphUrl, body, Encoding.UTF8, JsonMimeType, cancellationToken);
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