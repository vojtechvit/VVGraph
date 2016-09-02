using System;
using System.Collections.Generic;
using WebServices.AspNetCore.IntegrationTests.Graphs.Model;

namespace WebServices.AspNetCore.IntegrationTests.Graphs.SampleData
{
    public static class ValidGraphFactory
    {
        public static IEnumerable<Graph> CreateValidGraphs()
            => new[]
            {
                CreateGraphWithNodesAndEdges(),
                CreateGraphWithEmptyNodesAndEdges(),
                CreateGraphWithNullNodesAndEdges()
            };

        public static Graph CreateGraphWithNodesAndEdges()
            => new Graph
            {
                Name = Guid.NewGuid().ToString("N"),
                Nodes = new[]
                {
                    new Node { Id = 1, Label = "node1" },
                    new Node { Id = 2, Label = "node2" },
                    new Node { Id = 3, Label = "node3" },
                    new Node { Id = 4, Label = "node4" }
                },
                Edges = new[]
                {
                    new Edge { StartNodeId = 1, EndNodeId = 2 },
                    new Edge { StartNodeId = 2, EndNodeId = 3 },
                    new Edge { StartNodeId = 3, EndNodeId = 4 },
                    new Edge { StartNodeId = 3, EndNodeId = 1 }
                }
            };

        public static Graph CreateGraphWithEmptyNodesAndEdges()
            => new Graph
            {
                Name = "graph-with-empty-nodes-and-edges",
                Nodes = new Node[0],
                Edges = new Edge[0]
            };

        public static Graph CreateGraphWithNullNodesAndEdges()
            => new Graph
            {
                Name = "graph-with-null-nodes-and-edges",
                Nodes = null,
                Edges = null
            };
    }
}