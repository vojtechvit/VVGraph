using DataLoader.Contracts;
using DataLoader.Serialization.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebServices.ApiModel.Mappers.Contracts;
using WebServices.Proxy;
using WebServices.Proxy.Contracts;

namespace DataLoader
{
    public class DataLoader : IDataLoader
    {
        private readonly IFileSystemGraphDeserializer graphDeserializer;
        private readonly IGraphMapper graphMapper;
        private readonly IVVGraphClientFactory vvGraphClientFactory;

        public DataLoader(
            IFileSystemGraphDeserializer graphDeserializer,
            IGraphMapper graphMapper,
            IVVGraphClientFactory vvGraphClientFactory)
        {
            if (graphDeserializer == null)
                throw new ArgumentNullException(nameof(graphDeserializer));

            if (graphMapper == null)
                throw new ArgumentNullException(nameof(graphMapper));

            if (vvGraphClientFactory == null)
                throw new ArgumentNullException(nameof(vvGraphClientFactory));

            this.graphDeserializer = graphDeserializer;
            this.graphMapper = graphMapper;
            this.vvGraphClientFactory = vvGraphClientFactory;
        }

        public async Task LoadAsync(
            Uri baseUrl,
            string graphName,
            string directory,
            CancellationToken cancellationToken)
        {
            var vvGraphClientConfiguration = new VVGraphClientConfiguration()
            {
                BaseUrl = baseUrl
            };

            using (var vvGraphClient = vvGraphClientFactory.Create(vvGraphClientConfiguration))
            {
                var graph = graphDeserializer.Deserialize(
                    graphName,
                    directory);

                var apiModelGraph = graphMapper.Map(graph);

                await vvGraphClient.PutGraphAsync(apiModelGraph, cancellationToken);
            }
        }
    }
}