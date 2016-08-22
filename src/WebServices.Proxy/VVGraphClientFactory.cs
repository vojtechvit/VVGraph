using System;
using WebServices.Proxy.Contracts;

namespace WebServices.Proxy
{
    public sealed class VVGraphClientFactory : IVVGraphClientFactory
    {
        private readonly IHttpClient httpClient;
        private readonly IJsonSerializer jsonSerializer;
        private readonly IUrlHelper urlHelper;

        public VVGraphClientFactory(
            IHttpClient httpClient,
            IJsonSerializer jsonSerializer)
        {
            if (httpClient == null)
                throw new ArgumentNullException(nameof(httpClient));

            if (jsonSerializer == null)
                throw new ArgumentNullException(nameof(jsonSerializer));

            if (urlHelper == null)
                throw new ArgumentNullException(nameof(urlHelper));

            this.httpClient = httpClient;
            this.jsonSerializer = jsonSerializer;
            this.urlHelper = urlHelper;
        }

        public IVVGraphClient Create(VVGraphClientConfiguration configuration)
            => new VVGraphClient(configuration, httpClient, jsonSerializer, urlHelper);
    }
}