using Domain.Repositories.Contracts;
using Domain.Validation;
using System;
using System.Linq;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Threading.Tasks;
using System.Web;
using WebServices.ApiModel;
using WebServices.ApiModel.Mappers.Contracts;
using WebServices.Wcf.Contracts;

namespace WebServices.Wcf
{
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class GraphService : IGraphService
    {
        private readonly IRestHelper restHelper;
        private readonly IGraphRepository graphRepository;
        private readonly IApiModelGraphMapper graphMapper;

        public GraphService(
            IRestHelper restHelper,
            IGraphRepository graphRepository,
            IApiModelGraphMapper graphMapper)
        {
            if (restHelper == null)
                throw new ArgumentNullException(nameof(restHelper));

            if (graphRepository == null)
                throw new ArgumentNullException(nameof(graphRepository));

            if (graphMapper == null)
                throw new ArgumentNullException(nameof(graphMapper));

            this.restHelper = restHelper;
            this.graphRepository = graphRepository;
            this.graphMapper = graphMapper;
        }

        public async Task<Message> GetAsync(string graphName)
        {
            var graph = await graphRepository.GetAsync(graphName);

            if (graph == null)
            {
                return restHelper.NotFound();
            }

            return restHelper.Ok(graphMapper.Map(graph));
        }

        public async Task<Message> PutAsync(string graphName, Graph graph)
        {
            try
            {
                if (graphName != graph?.Name)
                {
                    return restHelper.BadRequest();
                }

                var domainGraph = graphMapper.Map(graph);

                // There could have been an extra Application layer that would
                // wrap this into a single operation, possibly even within a transaction.
                await graphRepository.DeleteAsync(domainGraph.Name);
                await graphRepository.CreateAsync(domainGraph);

                return restHelper.NoContent();
            }
            catch (ModelValidationException)
            {
                return restHelper.BadRequest();
            }
        }

        public async Task<Message> ShortestPathAsync(string graphName, int startNodeId, int endNodeId)
        {
            var graph = await graphRepository.GetAsync(graphName);

            if (graph == null)
            {
                return restHelper.NotFound();
            }

            var shortestPath = await graph.FindShortestPathAsync(graph.Nodes[startNodeId], graph.Nodes[endNodeId]);

            if (shortestPath == null)
            {
                return restHelper.NoContent();
            }

            return restHelper.Ok(shortestPath.Select(n => n.Id).ToArray());
        }
    }
}