using Domain.Validation;
using Domain.Validation.Contracts;
using System;
using System.Collections.Generic;

namespace Domain.Model
{
    public sealed class Node
    {
        private readonly ISet<NodeReference> adjacentNodes;

        private readonly INodeValidator nodeValidator;

        internal Node(
            string graphName,
            int id,
            string label,
            ISet<NodeReference> adjacentNodes,
            INodeValidator nodeValidator)
        {
            if (adjacentNodes == null)
                throw new ArgumentNullException(nameof(adjacentNodes));

            if (nodeValidator == null)
                throw new ArgumentNullException(nameof(nodeValidator));

            GraphName = graphName;
            Id = id;
            Label = label;
            this.adjacentNodes = adjacentNodes;
            this.nodeValidator = nodeValidator;
        }

        public string GraphName { get; }

        public int Id { get; }

        public string Label { get; }

        public IEnumerable<NodeReference> AdjacentNodes => adjacentNodes;

        public void AddAdjacentNode(int nodeId)
        {
            nodeValidator.ValidateId(nodeId).ThrowIfInvalid();
            nodeValidator.ValidateAdjacency(Id, nodeId).ThrowIfInvalid();

            adjacentNodes.Add(new NodeReference(GraphName, nodeId));
        }
    }
}