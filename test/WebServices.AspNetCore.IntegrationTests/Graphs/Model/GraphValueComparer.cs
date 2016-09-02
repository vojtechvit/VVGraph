using System.Collections.Generic;
using System.Linq;

namespace WebServices.AspNetCore.IntegrationTests.Graphs.Model
{
    public sealed class GraphValueComparer : IEqualityComparer<Graph>
    {
        public bool Equals(Graph x, Graph y)
            => x.Name == y.Name
            && x.Nodes.OrderBy(e => e.GetHashCode()).SequenceEqual(y.Nodes.OrderBy(e => e.GetHashCode()), new NodeValueComparer())
            && x.Edges.OrderBy(e => e.GetHashCode()).SequenceEqual(y.Edges.OrderBy(e => e.GetHashCode()), new EdgeValueComparer());

        public int GetHashCode(Graph obj) => obj.GetHashCode();
    }
}