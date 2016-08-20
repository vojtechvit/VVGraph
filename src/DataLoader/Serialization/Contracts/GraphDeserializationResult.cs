using Domain.Model;
using System;
using System.Collections.Generic;

namespace DataLoader.Serialization.Contracts
{
    public sealed class GraphDeserializationResult
    {
        internal GraphDeserializationResult(
            Graph graph,
            ICollection<Node> nodes)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (nodes == null)
                throw new ArgumentNullException(nameof(nodes));

            Graph = graph;
            Nodes = nodes;
        }

        public Graph Graph { get; }

        public ICollection<Node> Nodes { get; }
    }
}