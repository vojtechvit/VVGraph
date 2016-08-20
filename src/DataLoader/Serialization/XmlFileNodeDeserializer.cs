using DataLoader.Serialization.Contracts;
using Domain.Factories.Contracts;
using Domain.Model;
using System;

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

        public Node Deserialize(string path)
        {
            if (path == null)
                throw new ArgumentNullException(nameof(path));

            throw new NotImplementedException();
        }
    }
}