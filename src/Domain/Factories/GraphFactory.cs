using Domain.Algorithms.Contracts;
using Domain.Factories.Contracts;
using Domain.Model;
using Domain.Validation;
using Domain.Validation.Contracts;
using System;

namespace Domain.Factories
{
    public sealed class GraphFactory : IGraphFactory
    {
        private readonly IGraphValidator graphValidator;
        private readonly IPathFinder pathFinder;

        public GraphFactory(
            IGraphValidator graphValidator,
            IPathFinder pathFinder)
        {
            if (graphValidator == null)
                throw new ArgumentNullException(nameof(graphValidator));

            if (pathFinder == null)
                throw new ArgumentNullException(nameof(pathFinder));

            this.graphValidator = graphValidator;
            this.pathFinder = pathFinder;
        }

        public Graph Create(string name)
        {
            graphValidator.ValidateGraphName(name).ThrowIfInvalid();

            return new Graph(name, pathFinder);
        }
    }
}