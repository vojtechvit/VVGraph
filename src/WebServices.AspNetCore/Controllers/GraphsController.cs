using Domain.Repositories.Contracts;
using Domain.Validation;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using WebServices.ApiModel;
using WebServices.ApiModel.Mappers.Contracts;

namespace WebServices.AspNetCore.Controllers
{
    [Route("api/v1/graphs")]
    public class GraphsController : Controller
    {
        private readonly IGraphRepository graphRepository;
        private readonly INodeRepository nodeRepository;
        private readonly IGraphMapper graphMapper;
        private readonly INodeMapper nodeMapper;

        public GraphsController(
            IGraphRepository graphRepository,
            INodeRepository nodeRepository,
            IGraphMapper graphMapper,
            INodeMapper nodeMapper)
        {
            if (graphRepository == null)
                throw new System.ArgumentNullException(nameof(graphRepository));

            if (nodeRepository == null)
                throw new System.ArgumentNullException(nameof(nodeRepository));

            if (graphMapper == null)
                throw new System.ArgumentNullException(nameof(graphMapper));

            if (nodeMapper == null)
                throw new System.ArgumentNullException(nameof(nodeMapper));

            this.graphRepository = graphRepository;
            this.nodeRepository = nodeRepository;
            this.graphMapper = graphMapper;
            this.nodeMapper = nodeMapper;
        }

        // GET api/v1/graphs/{graphName}
        [HttpGet("{graphName}")]
        public async Task<Graph> GetAsync(string graphName)
        {
            var domainGraphTask = graphRepository.GetAsync(graphName);
            var domainNodesTask = nodeRepository.GetAllNodesForGraphAsync(graphName);

            await Task.WhenAll(domainGraphTask, domainNodesTask);

            return graphMapper.Map(domainGraphTask.Result, domainNodesTask.Result);
        }

        // PUT api/v1/graphs/{graphName}
        [HttpPut("{graphName}")]
        public async Task<IActionResult> PutAsync(string graphName, [FromBody]Graph graph)
        {
            try
            {
                if (graphName != graph?.Name)
                {
                    ModelState.AddModelError("name", "The graph name in the payload must match the name in the URL.");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var domainGraph = graphMapper.Map(graph);
                var domainNodes = nodeMapper.GetNodes(graph);

                await graphRepository.DeleteAsync(domainGraph.Name);
                await nodeRepository.DeleteAllForGraphAsync(domainGraph.Name);
                await nodeRepository.CreateAllAsync(domainNodes);
                await graphRepository.CreateAsync(domainGraph);

                return NoContent();
            }
            catch (ModelValidationException modelValidationException)
            {
                return BadRequest(new { Errors = modelValidationException.ValidationErrors });
            }
        }

        // GET api/v1/graphs/{graphName}/shortest-path?fromNodeId={fromNodeId}&toNodeId={toNodeId}
        [HttpGet("{graphName}/shortest-path")]
        public async Task<IActionResult> ShortestPathAsync(string graphName)
        {
            int startNodeId;
            int endNodeId;

            const string startNodeIdParameterName = "startNodeId";
            const string endNodeIdParameterName = "endNodeId";

            if (graphName == null
                || !Request.Query.ContainsKey(startNodeIdParameterName) || !int.TryParse(Request.Query[startNodeIdParameterName], out startNodeId)
                || !Request.Query.ContainsKey(endNodeIdParameterName) || !int.TryParse(Request.Query[endNodeIdParameterName], out endNodeId))
            {
                return NotFound();
            }

            var graph = await graphRepository.GetAsync(graphName);
            var shortestPath = await graph.GetShortestPathAsync(startNodeId, endNodeId);

            if (shortestPath == null)
            {
                return NoContent();
            }

            return Ok(shortestPath.Select(n => n.NodeId).ToArray());
        }
    }
}