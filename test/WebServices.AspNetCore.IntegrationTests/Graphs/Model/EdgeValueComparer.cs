using System.Collections.Generic;

namespace WebServices.AspNetCore.IntegrationTests.Graphs.Model
{
    public sealed class EdgeValueComparer : IEqualityComparer<Edge>
    {
        public bool Equals(Edge x, Edge y)
            => x.StartNodeId == y.StartNodeId
            && x.EndNodeId == y.EndNodeId;

        public int GetHashCode(Edge obj) => obj.GetHashCode();
    }
}