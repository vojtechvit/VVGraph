using Microsoft.AspNetCore.TestHost;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebServices.AspNetCore.IntegrationTests
{
    public static class RequestBuilderExtensions
    {
        public static Task<HttpResponseMessage> SendAsync(this RequestBuilder requestBuilder)
        {
            if (requestBuilder == null)
                throw new ArgumentNullException(nameof(requestBuilder));

            HttpMethod method = null;
            requestBuilder.And(request => { method = request.Method; });

            return requestBuilder.SendAsync(method.Method);
        }

        public static RequestBuilder Method(this RequestBuilder requestBuilder, string method)
        {
            if (requestBuilder == null)
                throw new ArgumentNullException(nameof(requestBuilder));

            requestBuilder.And(request => { request.Method = new HttpMethod(method); });

            return requestBuilder;
        }
    }
}