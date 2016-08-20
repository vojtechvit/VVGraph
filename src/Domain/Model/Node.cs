using System.Collections.Generic;
using System.Linq;

namespace Domain.Model
{
    public sealed class Node
    {
        internal Node(
            Graph graph,
            int id,
            string label)
        {
            Graph = graph;
            Id = id;
            Label = label;
        }

        internal Graph Graph { get; }

        public int Id { get; }

        public string Label { get; }

        public IEnumerable<Node> AdjacentNodes
            => Graph.Edges.Where(e => e.StartNode == this).Select(e => e.EndNode)
            .Union(Graph.Edges.Where(e => e.EndNode == this).Select(e => e.StartNode));

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
                hash = hash * 23 + Graph.Name.GetHashCode();
                hash = hash * 23 + Id.GetHashCode();
            }

            return hash;
        }
    }
}