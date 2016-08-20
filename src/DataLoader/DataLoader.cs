using DataLoader.Contracts;
using DataLoader.Serialization.Contracts;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebServices.ApiModel.Mappers.Contracts;
using WebServices.Proxy.Contracts;

namespace DataLoader
{
    public class DataLoader : IDataLoader
    {
        private readonly IFileSystemGraphDeserializer graphDeserializer;
        private readonly IGraphMapper graphMapper;
        private readonly IVVGraphClient vvGraphClient;

        public DataLoader(
            IFileSystemGraphDeserializer graphDeserializer,
            IGraphMapper graphMapper,
            IVVGraphClient vvGraphClient)
        {
            if (graphDeserializer == null)
                throw new ArgumentNullException(nameof(graphDeserializer));

            if (graphMapper == null)
                throw new ArgumentNullException(nameof(graphMapper));

            if (vvGraphClient == null)
                throw new ArgumentNullException(nameof(vvGraphClient));

            this.graphDeserializer = graphDeserializer;
            this.graphMapper = graphMapper;
            this.vvGraphClient = vvGraphClient;
        }

        public async Task LoadAsync(
            string graphName,
            string directory,
            CancellationToken cancellationToken)
        {
            var graphDeserializationResult = graphDeserializer.Deserialize(
                graphName,
                directory);

            var apiModelGraph = graphMapper.Map(
                graphDeserializationResult.Graph,
                graphDeserializationResult.Nodes);

            await vvGraphClient.PutGraphAsync(apiModelGraph, cancellationToken);
        }
    }
}