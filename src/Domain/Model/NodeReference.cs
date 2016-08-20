namespace Domain.Model
{
    public sealed class NodeReference
    {
        internal NodeReference(
            string graphName,
            int nodeId)
        {
            GraphName = graphName;
            NodeId = nodeId;
        }

        public string GraphName { get; }

        public int NodeId { get; }

        public override bool Equals(object obj)
        {
            var nodeReference = obj as NodeReference;

            if (!ReferenceEquals(nodeReference, null))
            {
                return Equals(nodeReference);
            }

            return base.Equals(obj);
        }

        public bool Equals(NodeReference nodeReference)
            => !ReferenceEquals(nodeReference, null) && GraphName == nodeReference.GraphName && NodeId == nodeReference.NodeId;

        public override int GetHashCode()
        {
            int hash = 17;

            unchecked
            {
                hash = hash * 23 + GraphName.GetHashCode();
                hash = hash * 23 + NodeId.GetHashCode();
            }

            return hash;
        }
    }
}