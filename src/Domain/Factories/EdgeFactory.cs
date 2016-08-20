using Domain.Factories.Contracts;
using Domain.Model;
using Domain.Validation;
using Domain.Validation.Contracts;
using System;

namespace Domain.Factories
{
    public class EdgeFactory : IEdgeFactory
    {
        private readonly IGraphValidator graphValidator;
        private readonly INodeValidator nodeValidator;

        public EdgeFactory(
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

        public Edge Create(
            string graphName,
            int startNodeId,
            int endNodeId)
        {
            graphValidator.ValidateName(graphName).ThrowIfInvalid();
            nodeValidator.ValidateId(startNodeId).ThrowIfInvalid();
            nodeValidator.ValidateId(endNodeId).ThrowIfInvalid();
            nodeValidator.ValidateAdjacency(startNodeId, endNodeId).ThrowIfInvalid();

            return new Edge(
                new NodeReference(graphName, startNodeId),
                new NodeReference(graphName, endNodeId));
        }
    }
}