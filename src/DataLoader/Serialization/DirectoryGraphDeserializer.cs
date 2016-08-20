using DataLoader.Serialization.Contracts;
using Domain.Factories.Contracts;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace DataLoader.Serialization
{
    public sealed class DirectoryGraphDeserializer : IFileSystemGraphDeserializer
    {
        private readonly IGraphFactory graphFactory;

        public DirectoryGraphDeserializer(
            IGraphFactory graphFactory)
        {
            if (graphFactory == null)
                throw new ArgumentNullException(nameof(graphFactory));

            this.graphFactory = graphFactory;
        }

        public Graph Deserialize(string graphName, string path)
        {
            if (graphName == null)
                throw new ArgumentNullException(nameof(graphName));

            if (path == null)
                throw new ArgumentNullException(nameof(path));

            var graph = graphFactory.Create(graphName);
            var edges = new List<Tuple<Node, int>>();

            foreach (var filePath in Directory.EnumerateFiles(path))
            {
                edges.AddRange(AddNodeToGraphAndReturnEdges(graph, filePath));
            }

            foreach (var edge in edges)
            {
                graph.AddEdge(edge.Item1, graph.Nodes[edge.Item2]);
            }

            return graph;
        }

        public IEnumerable<Tuple<Node, int>> AddNodeToGraphAndReturnEdges(Graph graph, string path)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph));

            if (path == null)
                throw new ArgumentNullException(nameof(path));

            var xdocument = XDocument.Load(path);
            var nodeElement = xdocument.Element("node");

            var id = int.Parse(nodeElement.Element("id").Value, CultureInfo.InvariantCulture);

            var label = nodeElement.Element("label").Value;

            var adjacentNodeIds = nodeElement
                .Element("adjacentNodes")
                .Elements()
                .Select(e => int.Parse(e.Value, CultureInfo.InvariantCulture));

            var node = graph.AddNode(id, label);

            return adjacentNodeIds.Select(adjId => Tuple.Create(node, adjId));
        }
    }
}