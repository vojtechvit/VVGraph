using System.Collections.Generic;

namespace WebServices.AspNetCore.IntegrationTests.Graphs.Model
{
    public sealed class NodeValueComparer : IEqualityComparer<Node>
    {
        public bool Equals(Node x, Node y)
            => x.Id == y.Id
            && x.Label == y.Label;

        public int GetHashCode(Node obj) => obj.GetHashCode();
    }
}