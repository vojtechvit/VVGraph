using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace WebServices.ApiModel
{
    [DataContract]
    public sealed class Edge
    {
        [DataMember]
        [Required]
        public int StartNodeId { get; set; }

        [DataMember]
        [Required]
        public int EndNodeId { get; set; }
    }
}