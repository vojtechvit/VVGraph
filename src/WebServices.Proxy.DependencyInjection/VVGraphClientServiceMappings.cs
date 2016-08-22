using Microsoft.Extensions.DependencyInjection;
using WebServices.Proxy.Contracts;

namespace WebServices.Proxy.DependencyInjection
{
    public static class VVGraphClientServiceMappings
    {
        public static IServiceCollection AddVVGraphClient(this IServiceCollection services)
            => services
                .AddSingleton<IVVGraphClientFactory, VVGraphClientFactory>()
                .AddSingleton<IJsonSerializer, JsonSerializerAdapter>()
                .AddSingleton<IUrlHelper, UrlHelper>()
                .AddSingleton<IHttpClient, HttpClientAdapter>();
    }
}