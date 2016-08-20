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
        private readonly IGraphMapper graphMapper;

        public GraphsController(
            IGraphRepository graphRepository,
            IGraphMapper graphMapper)
        {
            if (graphRepository == null)
                throw new System.ArgumentNullException(nameof(graphRepository));

            if (graphMapper == null)
                throw new System.ArgumentNullException(nameof(graphMapper));

            this.graphRepository = graphRepository;
            this.graphMapper = graphMapper;
        }

        // GET api/v1/graphs/{graphName}
        [HttpGet("{graphName}")]
        public async Task<Graph> GetAsync(string graphName)
        {
            var graph = await graphRepository.GetAsync(graphName);

            return graphMapper.Map(graph);
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

                await graphRepository.DeleteAsync(domainGraph.Name);
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
            var shortestPath = await graph.FindShortestPathAsync(graph.Nodes[startNodeId], graph.Nodes[endNodeId]);

            if (shortestPath == null)
            {
                return NoContent();
            }

            return Ok(shortestPath.Select(n => n.Id).ToArray());
        }
    }
}