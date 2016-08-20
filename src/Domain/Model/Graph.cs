using Domain.Algorithms.Contracts;
using System;
using System.Collections.Generic;

namespace Domain.Model
{
    public sealed class Graph
    {
        private readonly HashSet<NodeReference> nodes = new HashSet<NodeReference>();

        private readonly IPathFinder pathFinder;

        private readonly IEdgeEnumerator edgeEnumerator;

        internal Graph(
            string name,
            IPathFinder pathFinder,
            IEdgeEnumerator edgeEnumerator)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            Name = name;
            this.pathFinder = pathFinder;
            this.edgeEnumerator = edgeEnumerator;
        }

        public string Name { get; }

        public IReadOnlyCollection<NodeReference> Nodes => nodes;

        public IReadOnlyCollection<Edge> Edges
            => UnsupportedIfNull(edgeEnumerator).GetAllEdges(Name);

        public void AddNode(int nodeId)
        {
            nodes.Add(new NodeReference(Name, nodeId));
        }

        public Path GetShortestPath(int startNodeId, int endNodeId)
            => UnsupportedIfNull(pathFinder).GetShortestPath(Name, startNodeId, endNodeId);

        private static T UnsupportedIfNull<T>(T value)
        {
            if (value == null)
            {
                throw new NotImplementedException();
            }

            return value;
        }
    }
}