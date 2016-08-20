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
        private readonly INodeValidator nodeValidator;

        public PathFactory(
            IGraphValidator graphValidator,
            INodeValidator nodeValidator)
        {
            if (graphValidator == null)
                throw new ArgumentNullException(nameof(graphValidator));

            if (nodeValidator == null)
                throw new ArgumentNullException(nameof(nodeValidator));

            this.graphValidator = graphValidator;
            this.nodeValidator = nodeValidator;
        }

        public Path Create(IEnumerable<Node> nodes)
        {
            if (nodes == null)
                throw new ArgumentNullException(nameof(nodes));

            if (!nodes.Any())
                throw new ModelValidationException("A path must consist of at least one node.");

            var graph = nodes.FirstOrDefault()?.Graph;

            if (graph != null)
            {
                foreach (var node in nodes)
                {
                    nodeValidator.ValidateBelongingToGraph(graph, node);
                }
            }

            return new Path(nodes);
        }
    }
}