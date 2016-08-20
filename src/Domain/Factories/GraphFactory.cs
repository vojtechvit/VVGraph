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
        private readonly IEdgeEnumerator edgeEnumerator;

        public GraphFactory(
            IGraphValidator graphValidator,
            IPathFinder pathFinder,
            IEdgeEnumerator edgeEnumerator)
        {
            if (graphValidator == null)
                throw new ArgumentNullException(nameof(graphValidator));

            if (pathFinder == null)
                throw new ArgumentNullException(nameof(pathFinder));

            if (edgeEnumerator == null)
                throw new ArgumentNullException(nameof(edgeEnumerator));

            this.graphValidator = graphValidator;
            this.pathFinder = pathFinder;
            this.edgeEnumerator = edgeEnumerator;
        }

        public Graph Create(string name)
        {
            graphValidator.ValidateName(name).ThrowIfInvalid();

            return new Graph(name, pathFinder, edgeEnumerator);
        }
    }
}