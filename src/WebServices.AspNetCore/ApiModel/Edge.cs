using System.ComponentModel.DataAnnotations;

namespace WebServices.AspNetCore.ApiModel
{
    public sealed class Edge
    {
        [Required]
        public int StartNodeId { get; set; }

        [Required]
        public int EndNodeId { get; set; }
    }
}