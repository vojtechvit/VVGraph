﻿using System.Collections.Generic;
using WebServices.ApiModel;

namespace WebServices.Proxy.UnitTests.Helpers
{
    public static class DummyGraphFactory
    {
        public static IEnumerable<Graph> GetValidGraphs()
            => new[]
            {
                CreateGraphWithNodesAndEdges(),
                CreateGraphWithEmptyNodesAndEdges(),
                CreateGraphWithNullNodesAndEdges()
            };

        public static Graph CreateGraphWithNodesAndEdges()
            => new Graph
            {
                Name = "graph-with-nodes-and-edges",
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