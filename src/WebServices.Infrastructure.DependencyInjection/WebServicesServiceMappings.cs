using Domain.Algorithms.Contracts;
using Domain.Repositories.Contracts;
using Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using WebServices.ApiModel.Mappers;
using WebServices.ApiModel.Mappers.Contracts;
using WebServices.DataAccess.Neo4j;
using WebServices.DataAccess.Neo4j.Contracts;
using WebServices.DataAccess.Neo4j.DelegatedAlgorithms;
using WebServices.DataAccess.Neo4j.Repositories;

namespace WebServices.Infrastructure.DependencyInjection
{
    public static class WebServicesServiceMappings
    {
        public static IServiceCollection AddVVGraphWebServicesCommon(this IServiceCollection services)
        {
            // VV Graph Common
            services.AddVVGraphCommon();

            // Domain Repositories
            services.AddSingleton<IGraphRepository, Neo4jGraphRepository>();

            // Domain Delegated Algorithms
            services.Remove(services.First(sd => sd.ServiceType == typeof(IPathFinder)));
            services.AddSingleton<IPathFinder, Neo4jPathFinder>();

            // Api Model Mappers
            services.AddSingleton<IGraphMapper, GraphMapper>()
                .AddSingleton<INodeMapper, NodeMapper>()
                .AddSingleton<IEdgeMapper, EdgeMapper>();

            // Neo4j Infrastructure
            services.AddSingleton<INeo4jDriver, Neo4jDriver>();

            return services;
        }
    }
}