using System;

namespace Domain.Model
{
    public sealed class Edge
    {
        internal Edge(Node startNode, Node endNode)
        {
            if (startNode == null)
            {
                throw new ArgumentNullException(nameof(startNode));
            }

            if (endNode == null)
            {
                throw new ArgumentNullException(nameof(endNode));
            }

            StartNode = startNode;
            EndNode = endNode;
        }

        public Node StartNode { get; }

        public Node EndNode { get; }

        public override bool Equals(object obj)
        {
            var edge = obj as Edge;

            if (!ReferenceEquals(edge, null))
            {
                return Equals(edge);
            }

            return base.Equals(obj);
        }

        public bool Equals(Edge edge)
            => !ReferenceEquals(edge, null)
                && (StartNode.Equals(edge.StartNode) && EndNode.Equals(edge.EndNode))
                || (EndNode.Equals(edge.StartNode) && StartNode.Equals(edge.EndNode));

        public override int GetHashCode()
        {
            int hash;

            unchecked
            {
                hash = StartNode.GetHashCode() ^ 3 + EndNode.GetHashCode() ^ 3;
            }

            return hash;
        }
    }
}