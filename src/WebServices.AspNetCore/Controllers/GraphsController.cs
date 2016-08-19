using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebServices.AspNetCore.ApiModel;

namespace WebServices.AspNetCore.Controllers
{
    [Route("api/v1/graphs")]
    public class GraphsController : Controller
    {
        // GET api/v1/graphs/{graphName}
        [HttpGet("{graphName}")]
        public Graph Get(string graphName)
        {
            return new Graph
            {
                Name = graphName,
                Nodes = new[]
                {
                    new Node
                    {
                        Id = 1,
                        Label = "Facebook"
                    },
                    new Node
                    {
                        Id = 2,
                        Label = "Google"
                    }
                },
                Edges = new[]
                {
                    new Edge { StartNodeId = 1, EndNodeId = 2 }
                }
            };
        }

        // PUT api/v1/graphs/{graphName}
        [HttpPut("{graphName}")]
        public IActionResult Put(string graphName, [FromBody]Graph graph)
        {
            if (graphName != graph.Name)
            {
                ModelState.AddModelError("name", "The graph name in the payload must match the name in the URL.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // or just NoContent
            return CreatedAtAction(nameof(Get), graphName);
        }

        // GET api/v1/graphs/{graphName}/shortest-path?fromNodeId={fromNodeId}&toNodeId={toNodeId}
        [HttpGet("{graphName}/shortest-path")]
        public IActionResult ShortestPath(string graphName)
        {
            int fromNodeId;
            int toNodeId;

            if (graphName == null
                || !Request.Query.ContainsKey("fromNodeId") || !int.TryParse(Request.Query["fromNodeId"], out fromNodeId)
                || !Request.Query.ContainsKey("toNodeId") || !int.TryParse(Request.Query["toNodeId"], out toNodeId))
            {
                return NotFound();
            }

            return Ok(new[] { 1, 2, 3 });
        }
    }
}