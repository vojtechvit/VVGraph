using System.ComponentModel.DataAnnotations;

namespace WebServices.ApiModel
{
    public sealed class Edge
    {
        [Required]
        public int StartNodeId { get; set; }

        [Required]
        public int EndNodeId { get; set; }
    }
}