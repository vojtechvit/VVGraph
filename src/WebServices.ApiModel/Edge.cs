using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace WebServices.ApiModel
{
    [DataContract]
    public sealed class Edge
    {
        [DataMember(Name = "startNodeId")]
        [Required]
        public int StartNodeId { get; set; }

        [DataMember(Name = "endNodeId")]
        [Required]
        public int EndNodeId { get; set; }
    }
}