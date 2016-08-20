using Domain.Factories.Contracts;
using Domain.Validation;
using Domain.Validation.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Model
{
    public sealed class Node
    {
        private readonly ISet<NodeReference> adjacentNodes;
        private readonly INodeValidator nodeValidator;
        private readonly IEdgeFactory edgeFactory;

        internal Node(
            string graphName,
            int id,
            string label,
            ISet<NodeReference> adjacentNodes,
            INodeValidator nodeValidator,
            IEdgeFactory edgeFactory)
        {
            if (adjacentNodes == null)
                throw new ArgumentNullException(nameof(adjacentNodes));

            if (nodeValidator == null)
                throw new ArgumentNullException(nameof(nodeValidator));

            if (edgeFactory == null)
                throw new ArgumentNullException(nameof(edgeFactory));

            GraphName = graphName;
            Id = id;
            Label = label;
            this.adjacentNodes = adjacentNodes;
            this.nodeValidator = nodeValidator;
            this.edgeFactory = edgeFactory;
        }

        public string GraphName { get; }

        public int Id { get; }

        public string Label { get; }

        public IEnumerable<NodeReference> AdjacentNodes => adjacentNodes;

        public IEnumerable<Edge> Edges => AdjacentNodes.Select(adjNode => edgeFactory.Create(GraphName, Id, adjNode.NodeId));

        public void AddAdjacentNode(int nodeId)
        {
            nodeValidator.ValidateId(nodeId).ThrowIfInvalid();
            nodeValidator.ValidateAdjacency(Id, nodeId).ThrowIfInvalid();

            adjacentNodes.Add(new NodeReference(GraphName, nodeId));
        }
    }
}