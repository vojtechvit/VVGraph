﻿using System.ComponentModel.DataAnnotations;

namespace WebServices.ApiModel
{
    public sealed class Node
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Label { get; set; }
    }
}