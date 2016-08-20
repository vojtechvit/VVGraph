using DataLoader.Contracts;
using DataLoader.Serialization;
using DataLoader.Serialization.Contracts;
using Domain.Algorithms.Contracts;
using Infrastructure.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using WebServices.ApiModel.Mappers;
using WebServices.ApiModel.Mappers.Contracts;
using WebServices.Proxy.DependencyInjection;

namespace DataLoader
{
    public static class DataLoaderServiceMappings
    {
        public static IServiceCollection AddVVGraphDataLoader(
            this IServiceCollection services)
            => services
                // VV Graph Common
                .AddVVGraphCommon()

                // Serializers
                .AddSingleton<IFileSystemGraphDeserializer, DirectoryGraphDeserializer>()

                // Api Model Mappers
                .AddSingleton<IGraphMapper, GraphMapper>()
                .AddSingleton<INodeMapper, NodeMapper>()
                .AddSingleton<IEdgeMapper, EdgeMapper>()

                // Data Loader
                .AddSingleton<IDataLoader, DataLoader>()

                // VV Graph Client
                .AddVVGraphClient();
    }
}