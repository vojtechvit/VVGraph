using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebServices.Proxy.Contracts
{
    public interface IHttpClient : IDisposable
    {
        Task<HttpResponseMessage> PutAsync(
            Uri requestUri,
            string content,
            Encoding contentEncoding,
            string contentType,
            CancellationToken cancellationToken);
    }
}