using System;
using WebServices.Proxy.Contracts;

namespace WebServices.Proxy
{
    public sealed class UrlHelper : IUrlHelper
    {
        public Uri GetGraphUrl(Uri baseUrl, string graphName)
        {
            if (baseUrl == null)
                throw new ArgumentNullException(nameof(baseUrl));

            if (graphName == null)
                throw new ArgumentNullException(nameof(graphName));

            return new Uri(baseUrl, $"graphs/{graphName}");
        }
    }
}