using Microsoft.Extensions.DependencyInjection;
using System;
using System.Configuration;
using WebServices.DataAccess.Neo4j;
using WebServices.Infrastructure.DependencyInjection;
using WebServices.Wcf.Contracts;

namespace WebServices.Wcf.DependencyInjection
{
    public static class ServiceProvider
    {
        public static IServiceProvider Instance { get; } = CreateServiceProvider();

        private static IServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();

            services.AddTransient<GraphService>();

            services.AddSingleton<IRestHelper, RestHelper>();
            services.AddVVGraphWebServicesCommon();
            services.AddSingleton(new Neo4jConfiguration { Uri = ConfigurationManager.AppSettings["neo4j:url"] });

            return services.BuildServiceProvider();
        }
    }
}