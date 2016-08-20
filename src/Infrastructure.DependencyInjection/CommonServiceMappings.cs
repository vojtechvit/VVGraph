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
                .AddSingleton<INodeFactory, NodeFactory>()
                .AddSingleton<IPathFactory, PathFactory>();
    }
}