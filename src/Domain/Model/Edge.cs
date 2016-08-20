using System;

namespace Domain.Model
{
    public sealed class Edge
    {
        internal Edge(
            Graph graph,
            Node startNode,
            Node endNode)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (startNode == null)
                throw new ArgumentNullException(nameof(startNode));

            if (endNode == null)
                throw new ArgumentNullException(nameof(endNode));

            Graph = graph;
            StartNode = startNode;
            EndNode = endNode;
        }

        internal Graph Graph { get; }

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
                && ((StartNode.Equals(edge.StartNode) && EndNode.Equals(edge.EndNode))
                    || (EndNode.Equals(edge.StartNode) && StartNode.Equals(edge.EndNode)));

        public override int GetHashCode()
        {
            int hash = 17;

            int first = StartNode.Id < EndNode.Id ? StartNode.Id : EndNode.Id;
            int second = StartNode.Id < EndNode.Id ? EndNode.Id : StartNode.Id;

            unchecked
            {
                hash = hash * 23 + StartNode.Graph.Name.GetHashCode();
                hash = hash * 23 + first;
                hash = hash * 23 + second;
            }

            return hash;
        }
    }
}