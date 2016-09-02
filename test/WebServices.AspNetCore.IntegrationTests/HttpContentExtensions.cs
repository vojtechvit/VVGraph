using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebServices.AspNetCore.IntegrationTests
{
    public static class HttpContentExtensions
    {
        public static async Task<T> ReadAsJsonAsync<T>(this HttpContent httpContent)
        {
            if (httpContent == null)
                throw new ArgumentNullException(nameof(httpContent));

            return JsonConvert.DeserializeObject<T>(await httpContent.ReadAsStringAsync());
        }
    }
}