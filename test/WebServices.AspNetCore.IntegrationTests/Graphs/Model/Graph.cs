using System.Collections.Generic;

namespace WebServices.AspNetCore.IntegrationTests.Graphs.Model
{
    public sealed class Graph
    {
        public string Name { get; set; }

        public IList<Node> Nodes { get; set; }

        public IList<Edge> Edges { get; set; }
    }
}