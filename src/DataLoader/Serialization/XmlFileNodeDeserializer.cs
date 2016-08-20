using DataLoader.Serialization.Contracts;
using Domain.Factories.Contracts;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;

namespace DataLoader.Serialization
{
    public sealed class XmlFileNodeDeserializer : IFileSystemNodeDeserializer
    {
        private readonly IFileSystemNodeDeserializer fileSystemNodeDeserializer;
        private readonly INodeFactory nodeFactory;

        public XmlFileNodeDeserializer(
            IFileSystemNodeDeserializer fileSystemNodeDeserializer,
            INodeFactory nodeFactory)
        {
            if (fileSystemNodeDeserializer == null)
                throw new ArgumentNullException(nameof(fileSystemNodeDeserializer));

            if (nodeFactory == null)
                throw new ArgumentNullException(nameof(nodeFactory));

            this.fileSystemNodeDeserializer = fileSystemNodeDeserializer;
            this.nodeFactory = nodeFactory;
        }

        public Node Deserialize(string graphName, string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            var xdocument = XDocument.Load(path);

            var id = int.Parse(xdocument.Element("id").Value, CultureInfo.InvariantCulture);

            var label = xdocument.Element("label").Value;

            var adjacentNodeIds = new HashSet<int>(
                xdocument
                    .Element("adjacentNodes")
                    .Elements()
                    .Select(e => int.Parse(e.Value, CultureInfo.InvariantCulture)));

            return nodeFactory.Create(graphName, id, label, adjacentNodeIds);
        }
    }
}