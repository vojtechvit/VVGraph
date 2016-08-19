using Domain.Algorithms.Contracts;
using Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Model
{
    public sealed class Graph
    {
        private readonly HashSet<Node> _nodes = new HashSet<Node>();

        private readonly HashSet<Edge> _edges = new HashSet<Edge>();

        private readonly IPathFinder _pathFinder;

        internal Graph(
            string name,
            IPathFinder pathFinder)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (pathFinder == null)
            {
                throw new ArgumentNullException(nameof(pathFinder));
            }

            Name = name;
            _pathFinder = pathFinder;
        }

        public string Name { get; }

        public IReadOnlyCollection<Node> Nodes => _nodes;

        public IReadOnlyCollection<Edge> Edges => _edges;

        public void AddNode(int id, string label)
        {
            if (id <= 0)
            {
                throw new ModelValidationException("Node id must be greater than 0.");
            }

            if (label == null)
            {
                throw new ModelValidationException("Node label is required.");
            }

            if (label == string.Empty)
            {
                throw new ModelValidationException("Node label must not be empty.");
            }

            _nodes.Add(new Node(this, id, label));
        }

        public void AddEdge(Node startNode, Node endNode)
        {
            if (startNode == null)
            {
                throw new ArgumentNullException(nameof(startNode));
            }

            if (endNode == null)
            {
                throw new ArgumentNullException(nameof(endNode));
            }

            if (startNode.Graph != this)
            {
                throw new ModelValidationException("Invalid start node.");
            }

            if (endNode.Graph != this)
            {
                throw new ModelValidationException("Invalid end node.");
            }

            _edges.Add(new Edge(startNode, endNode));
        }

        public Path GetPath(IEnumerable<Node> nodes)
        {
            if (nodes == null)
            {
                throw new ArgumentNullException(nameof(nodes));
            }

            if (!nodes.Any())
            {
                throw new ModelValidationException("A path must consist of at least one node.");
            }

            if (nodes.Any(n => n.Graph != this))
            {
                throw new ModelValidationException("Some of the nodes in the path are invalid.");
            }

            return new Path(nodes.ToList());
        }

        public Path GetShortestPath(Node startNode, Node endNode)
        {
            if (startNode == null)
            {
                throw new ArgumentNullException(nameof(startNode));
            }

            if (endNode == null)
            {
                throw new ArgumentNullException(nameof(endNode));
            }

            if (startNode.Graph != this)
            {
                throw new ModelValidationException("Invalid start node.");
            }

            if (endNode.Graph != this)
            {
                throw new ModelValidationException("Invalid end node.");
            }

            return _pathFinder.GetShortestPath(this, startNode, endNode);
        }
    }
}