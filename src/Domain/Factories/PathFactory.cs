using Domain.Factories.Contracts;
using Domain.Model;
using Domain.Validation;
using Domain.Validation.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Factories
{
    public sealed class PathFactory : IPathFactory
    {
        private readonly IGraphValidator graphValidator;

        public PathFactory(IGraphValidator graphValidator)
        {
            if (graphValidator == null)
                throw new ArgumentNullException(nameof(graphValidator));

            this.graphValidator = graphValidator;
        }

        public Path Create(string graphName, IEnumerable<int> nodeIds)
        {
            if (nodeIds == null)
                throw new ArgumentNullException(nameof(nodeIds));

            graphValidator.ValidateGraphName(graphName).ThrowIfInvalid();

            if (!nodeIds.Any())
                throw new ModelValidationException("A path must consist of at least one node.");

            return new Path(nodeIds.Select(nodeId => new NodeReference(graphName, nodeId)));
        }
    }
}