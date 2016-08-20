﻿using Domain.Factories.Contracts;
using Domain.Model;
using Domain.Validation;
using Domain.Validation.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Factories
{
    public sealed class NodeFactory : INodeFactory
    {
        private readonly IGraphValidator graphValidator;

        private readonly INodeValidator nodeValidator;

        public NodeFactory(
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

        public Node Create(string graphName, int id, string label, ISet<int> adjacentNodeIds)
        {
            graphValidator.ValidateName(graphName).ThrowIfInvalid();
            nodeValidator.ValidateId(id).ThrowIfInvalid();
            nodeValidator.ValidateLabel(label).ThrowIfInvalid();

            foreach (var nodeId in adjacentNodeIds)
            {
                nodeValidator.ValidateId(nodeId).ThrowIfInvalid();
                nodeValidator.ValidateAdjacency(id, nodeId).ThrowIfInvalid();
            }

            return new Node(
                graphName,
                id,
                label,
                new HashSet<NodeReference>(adjacentNodeIds.Select(nodeId => new NodeReference(graphName, nodeId))),
                nodeValidator);
        }
    }
}