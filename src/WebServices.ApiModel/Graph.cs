using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace WebServices.ApiModel
{
    [DataContract]
    public sealed class Graph
    {
        [DataMember(Name = "name")]
        [Required]
        public string Name { get; set; }

        [DataMember(Name = "nodes")]
        public IEnumerable<Node> Nodes { get; set; }

        [DataMember(Name = "edges")]
        public IEnumerable<Edge> Edges { get; set; }
    }
}