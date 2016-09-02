using Xunit;

namespace WebServices.AspNetCore.IntegrationTests.Graphs.Model
{
    public static class GraphAssert
    {
        public static void Equal(Graph expected, Graph actual)
        {
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.Nodes, actual.Nodes, new NodeValueComparer());
            Assert.Equal(expected.Edges, actual.Edges, new EdgeValueComparer());
        }
    }
}