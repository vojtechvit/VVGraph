using DataLoader.Serialization.Contracts;
using Domain.Factories.Contracts;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataLoader.Serialization
{
    public sealed class DirectoryGraphDeserializer : IFileSystemGraphDeserializer
    {
        private readonly IFileSystemNodeDeserializer fileSystemNodeDeserializer;
        private readonly IGraphFactory graphFactory;

        public DirectoryGraphDeserializer(
            IFileSystemNodeDeserializer fileSystemNodeDeserializer,
            IGraphFactory graphFactory)
        {
            if (fileSystemNodeDeserializer == null)
                throw new ArgumentNullException(nameof(fileSystemNodeDeserializer));

            if (graphFactory == null)
                throw new ArgumentNullException(nameof(graphFactory));

            this.fileSystemNodeDeserializer = fileSystemNodeDeserializer;
            this.graphFactory = graphFactory;
        }

        public GraphDeserializationResult Deserialize(string graphName, string path)
        {
            if (graphName == null)
                throw new ArgumentNullException(nameof(graphName));

            if (path == null)
                throw new ArgumentNullException(nameof(path));

            var graph = graphFactory.Create(graphName);
            var nodes = new List<Node>();

            foreach (var filePath in Directory.EnumerateFiles(path))
            {
                var node = fileSystemNodeDeserializer.Deserialize(graphName, filePath);

                graph.AddNode(node.Id);

                nodes.Add(node);
            }

            return new GraphDeserializationResult(graph, nodes);
        }
    }
}