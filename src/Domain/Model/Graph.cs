using Domain.Algorithms.Contracts;
using Domain.Validation;
using Domain.Validation.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Model
{
    public sealed class Graph
    {
        private readonly Dictionary<int, Node> nodes = new Dictionary<int, Node>();
        private readonly HashSet<Edge> edges = new HashSet<Edge>();
        private readonly INodeValidator nodeValidator;
        private readonly IPathFinder pathFinder;

        internal Graph(
            string name,
            INodeValidator nodeValidator,
            IPathFinder pathFinder)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (nodeValidator == null)
                throw new ArgumentNullException(nameof(nodeValidator));

            if (pathFinder == null)
                throw new ArgumentNullException(nameof(pathFinder));

            Name = name;
            this.nodeValidator = nodeValidator;
            this.pathFinder = pathFinder;
        }

        public string Name { get; }

        public IReadOnlyDictionary<int, Node> Nodes => nodes;

        public IReadOnlyCollection<Edge> Edges => edges;

        public Node AddNode(int id, string label)
        {
            nodeValidator.ValidateId(id).ThrowIfInvalid();
            nodeValidator.ValidateLabel(label).ThrowIfInvalid();

            if (nodes.ContainsKey(id))
            {
                throw new ModelValidationException("A node with this key already exists");
            }

            var node = new Node(this, id, label);

            nodes[id] = node;

            return node;
        }

        public Edge AddEdge(Node startNode, Node endNode)
        {
            nodeValidator.ValidateBelongingToGraph(this, startNode).ThrowIfInvalid();
            nodeValidator.ValidateBelongingToGraph(this, endNode).ThrowIfInvalid();

            var edge = new Edge(this, startNode, endNode);

            edges.Add(edge);

            return edge;
        }

        public Task<Path> FindShortestPathAsync(Node startNode, Node endNode)
            => pathFinder.FindShortestPathAsync(this, startNode, endNode);
    }
}