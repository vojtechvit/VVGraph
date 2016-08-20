using Domain.Algorithms.Contracts;
using Domain.Repositories.Contracts;
using Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
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
            => services
                // VV Graph Common
                .AddVVGraphCommon()

                // Domain Repositories
                .AddSingleton<IGraphRepository, Neo4jGraphRepository>()
                .AddSingleton<INodeRepository, Neo4jNodeRepository>()

                // Domain Delegated Algorithms
                .AddSingleton<IPathFinder, Neo4jPathFinder>()
                .AddSingleton<IEdgeEnumerator, Neo4jEdgeEnumerator>()

                // Api Model Mappers
                .AddSingleton<IGraphMapper, GraphMapper>()
                .AddSingleton<INodeMapper, NodeMapper>()

                // Neo4j Infrastructure
                .AddSingleton<INeo4jDriver, Neo4jDriver>();
    }
}