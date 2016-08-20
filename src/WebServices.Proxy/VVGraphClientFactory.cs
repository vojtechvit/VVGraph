using System;
using WebServices.Proxy.Contracts;

namespace WebServices.Proxy
{
    public sealed class VVGraphClientFactory : IVVGraphClientFactory
    {
        private readonly IHttpClient httpClient;
        private readonly IJsonSerializer jsonSerializer;

        public VVGraphClientFactory(
            IHttpClient httpClient,
            IJsonSerializer jsonSerializer)
        {
            if (httpClient == null)
                throw new ArgumentNullException(nameof(httpClient));

            if (jsonSerializer == null)
                throw new ArgumentNullException(nameof(jsonSerializer));

            this.httpClient = httpClient;
            this.jsonSerializer = jsonSerializer;
        }

        public IVVGraphClient Create(VVGraphClientConfiguration configuration)
            => new VVGraphClient(configuration, httpClient, jsonSerializer);
    }
}