using System;

namespace Domain.Model
{
    public sealed class Node
    {
        internal Node(Graph graph, int id, string label)
        {
            if (graph == null)
            {
                throw new ArgumentNullException(nameof(graph));
            }

            Graph = graph;
            Id = id;
            Label = label;
        }

        public int Id { get; }

        public string Label { get; }

        internal Graph Graph { get; }

        public override bool Equals(object obj)
        {
            var node = obj as Node;

            if (!ReferenceEquals(node, null))
            {
                return Equals(node);
            }

            return base.Equals(obj);
        }

        public bool Equals(Node node)
            => !ReferenceEquals(node, null) && Graph == node.Graph && Id == node.Id;

        public override int GetHashCode()
        {
            int hash = 17;

            unchecked
            {
                hash = hash * 23 + Graph.GetHashCode();
                hash = hash * 23 + Id.GetHashCode();
            }

            return hash;
        }
    }
}