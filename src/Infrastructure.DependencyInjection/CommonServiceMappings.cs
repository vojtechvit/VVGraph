using Domain.Algorithms;
using Domain.Algorithms.Contracts;
using Domain.Factories;
using Domain.Factories.Contracts;
using Domain.Validation;
using Domain.Validation.Contracts;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection
{
    public static class CommonServiceMappings
    {
        public static IServiceCollection AddVVGraphCommon(this IServiceCollection services)
            => services

                // Domain Validators
                .AddSingleton<IGraphValidator, GraphValidator>()
                .AddSingleton<INodeValidator, NodeValidator>()

                // Domain Factories
                .AddSingleton<IGraphFactory, GraphFactory>()
                .AddSingleton<IPathFactory, PathFactory>()

                // Domain Delegated Algorithms
                .AddSingleton<IPathFinder, DummyPathFinder>();
    }
}