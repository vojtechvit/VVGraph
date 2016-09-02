using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace WebServices.AspNetCore.IntegrationTests
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection ReplaceWithSingleton<TService>(this IServiceCollection services, TService implementationInstance)
            where TService : class
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.Where(s => s.ServiceType == typeof(TService)).ToList().ForEach(sd => services.Remove(sd));
            services.AddSingleton(implementationInstance);

            return services;
        }
    }
}