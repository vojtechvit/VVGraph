using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebServices.Proxy.Contracts;

namespace WebServices.Proxy
{
    public sealed class HttpClientAdapter : HttpClient, IHttpClient
    {
        public Task<HttpResponseMessage> PutAsync(
            Uri requestUri,
            string content,
            Encoding contentEncoding,
            string contentType,
            CancellationToken cancellationToken)
        {
            var httpContent = new StringContent(content, contentEncoding, contentType);

            return PutAsync(requestUri, httpContent, cancellationToken);
        }
    }
}