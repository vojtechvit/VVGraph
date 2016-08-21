using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace WebServices.ApiModel
{
    [DataContract]
    public sealed class Node
    {
        [DataMember(Name = "id")]
        [Required]
        public int Id { get; set; }

        [DataMember(Name = "label")]
        [Required]
        public string Label { get; set; }
    }
}