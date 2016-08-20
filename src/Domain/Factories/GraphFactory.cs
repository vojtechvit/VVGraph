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
        private readonly INodeValidator nodeValidator;
        private readonly IPathFinder pathFinder;

        public GraphFactory(
            IGraphValidator graphValidator,
            INodeValidator nodeValidator,
            IPathFinder pathFinder)
        {
            if (graphValidator == null)
                throw new ArgumentNullException(nameof(graphValidator));

            if (nodeValidator == null)
                throw new ArgumentNullException(nameof(nodeValidator));

            if (pathFinder == null)
                throw new ArgumentNullException(nameof(pathFinder));

            this.graphValidator = graphValidator;
            this.nodeValidator = nodeValidator;
            this.pathFinder = pathFinder;
        }

        public Graph Create(string name)
        {
            graphValidator.ValidateName(name).ThrowIfInvalid();

            return new Graph(name, nodeValidator, pathFinder);
        }
    }
}