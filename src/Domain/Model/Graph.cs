using Domain.Algorithms.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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

        public Task<IReadOnlyCollection<Edge>> GetAllEdgesAsync
            => UnsupportedIfNull(edgeEnumerator).GetAllEdgesAsync(Name);

        public void AddNode(int nodeId)
        {
            nodes.Add(new NodeReference(Name, nodeId));
        }

        public Task<Path> GetShortestPathAsync(int startNodeId, int endNodeId)
            => UnsupportedIfNull(pathFinder).GetShortestPathAsync(Name, startNodeId, endNodeId);

        private static T UnsupportedIfNull<T>(T value)
        {
            if (value == null)
            {
                throw new NotImplementedException("The implementation of the operation was not injected.");
            }

            return value;
        }
    }
}