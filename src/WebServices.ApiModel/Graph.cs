using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace WebServices.ApiModel
{
    [DataContract]
    public sealed class Graph
    {
        [DataMember]
        [Required]
        public string Name { get; set; }

        [DataMember]
        public IEnumerable<Node> Nodes { get; set; }

        [DataMember]
        public IEnumerable<Edge> Edges { get; set; }
    }
}