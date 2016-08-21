using Domain.Repositories.Contracts;
using Domain.Validation;
using Microsoft.AspNetCore.Mvc;
using System;
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
        private readonly IApiModelGraphMapper graphMapper;

        public GraphsController(
            IGraphRepository graphRepository,
            IApiModelGraphMapper graphMapper)
        {
            if (graphRepository == null)
                throw new ArgumentNullException(nameof(graphRepository));

            if (graphMapper == null)
                throw new ArgumentNullException(nameof(graphMapper));

            this.graphRepository = graphRepository;
            this.graphMapper = graphMapper;
        }

        // GET api/v1/graphs/{graphName}
        [HttpGet("{graphName}")]
        public async Task<IActionResult> GetAsync(string graphName)
        {
            var graph = await graphRepository.GetAsync(graphName);

            if (graph == null)
            {
                return NotFound();
            }

            return Ok(graphMapper.Map(graph));
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

                // There could have been an extra Application layer that would
                // wrap this into a single operation, possibly even within a transaction.
                await graphRepository.DeleteAsync(domainGraph.Name);
                await graphRepository.CreateAsync(domainGraph);

                return NoContent();
            }
            catch (ModelValidationException modelValidationException)
            {
                return BadRequest(new { Errors = modelValidationException.ValidationErrors });
            }
        }

        // GET api/v1/graphs/{graphName}/shortest-path?startNodeId={startNodeId}&endNodeId={endNodeId}
        [HttpGet("{graphName}/shortest-path")]
        public async Task<IActionResult> ShortestPathAsync(string graphName, int? startNodeId, int? endNodeId)
        {
            if (graphName == null || !startNodeId.HasValue || !endNodeId.HasValue)
            {
                return NotFound();
            }

            var graph = await graphRepository.GetAsync(graphName);

            if (graph == null)
            {
                return NotFound();
            }

            var shortestPath = await graph.FindShortestPathAsync(
                graph.Nodes[startNodeId.Value],
                graph.Nodes[endNodeId.Value]);

            if (shortestPath == null)
            {
                return NoContent();
            }

            return Ok(shortestPath.Select(n => n.Id).ToArray());
        }
    }
}