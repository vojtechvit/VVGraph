using Microsoft.Extensions.DependencyInjection;
using WebServices.Proxy.Contracts;

namespace WebServices.Proxy.DependencyInjection
{
    public static class VVGraphClientServiceMappings
    {
        public static IServiceCollection AddVVGraphClient(this IServiceCollection services)
            => services
                .AddSingleton<IVVGraphClient, VVGraphClient>()
                .AddSingleton<IJsonSerializer, JsonSerializerAdapter>()
                .AddSingleton<IHttpClient, HttpClientAdapter>();
    }
}