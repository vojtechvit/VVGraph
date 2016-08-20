using Domain.Algorithms.Contracts;
using System;
using System.Collections.Generic;

namespace Domain.Model
{
    public sealed class Graph
    {
        private readonly HashSet<NodeReference> nodes = new HashSet<NodeReference>();

        private readonly HashSet<Edge> edges = new HashSet<Edge>();

        private readonly IPathFinder pathFinder;

        internal Graph(
            string name,
            IPathFinder pathFinder)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (pathFinder == null)
                throw new ArgumentNullException(nameof(pathFinder));

            Name = name;
            this.pathFinder = pathFinder;
        }

        public string Name { get; }

        public IReadOnlyCollection<NodeReference> Nodes => nodes;

        public IReadOnlyCollection<Edge> Edges => edges;

        public void AddNode(int nodeId)
        {
            nodes.Add(new NodeReference(Name, nodeId));
        }

        public void AddEdge(int startNodeId, int endNodeId)
        {
            var startNodeReference = new NodeReference(Name, startNodeId);
            var endNodeReference = new NodeReference(Name, endNodeId);

            edges.Add(new Edge(startNodeReference, endNodeReference));
        }

        public Path GetShortestPath(int startNodeId, int endNodeId)
            => pathFinder.GetShortestPath(Name, startNodeId, endNodeId);
    }
}