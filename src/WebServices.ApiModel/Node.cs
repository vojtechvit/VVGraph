using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace WebServices.ApiModel
{
    [DataContract]
    public sealed class Node
    {
        [DataMember]
        [Required]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string Label { get; set; }
    }
}