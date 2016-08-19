using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebServices.AspNetCore.ApiModel
{
    public sealed class Graph
    {
        [Required]
        public string Name { get; set; }

        public IEnumerable<Node> Nodes { get; set; }

        public IEnumerable<Edge> Edges { get; set; }
    }
}
