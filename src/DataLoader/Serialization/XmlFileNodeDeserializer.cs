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
        private readonly INodeFactory nodeFactory;

        public XmlFileNodeDeserializer(
            INodeFactory nodeFactory)
        {
            if (nodeFactory == null)
                throw new ArgumentNullException(nameof(nodeFactory));

            this.nodeFactory = nodeFactory;
        }

        public Node Deserialize(string graphName, string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            var xdocument = XDocument.Load(path);
            var nodeElement = xdocument.Element("node");

            var id = int.Parse(nodeElement.Element("id").Value, CultureInfo.InvariantCulture);

            var label = nodeElement.Element("label").Value;

            var adjacentNodeIds = new HashSet<int>(
                nodeElement
                    .Element("adjacentNodes")
                    .Elements()
                    .Select(e => int.Parse(e.Value, CultureInfo.InvariantCulture)));

            return nodeFactory.Create(graphName, id, label, adjacentNodeIds);
        }
    }
}