using Domain.Algorithms.Contracts;
using Domain.Repositories.Contracts;
using Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Neo4jClient;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using WebServices.ApiModel.Mappers;
using WebServices.ApiModel.Mappers.Contracts;
using WebServices.DataAccess.Neo4j;
using WebServices.DataAccess.Neo4j.DelegatedAlgorithms;
using WebServices.DataAccess.Neo4j.Mappers;
using WebServices.DataAccess.Neo4j.Mappers.Contracts;
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
            services.AddSingleton<IApiModelGraphMapper, ApiModelGraphMapper>()
                .AddSingleton<IApiModelNodeMapper, ApiModelNodeMapper>()
                .AddSingleton<IApiModelEdgeMapper, ApiModelEdgeMapper>();

            // Neo4j Model Mappers
            services.AddSingleton<INeo4jGraphMapper, Neo4jGraphMapper>();

            // Neo4j Infrastructure
            services.AddSingleton<IGraphClient>(sp =>
            {
                var configuration = sp.GetService<Neo4jConfiguration>();
                var graphClient = new GraphClient(new Uri(configuration.Uri));
                graphClient.JsonContractResolver = new CamelCasePropertyNamesContractResolver();
                graphClient.Connect();
                return graphClient;
            });

            return services;
        }
    }
}