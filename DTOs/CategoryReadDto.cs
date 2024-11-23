using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace web_api.DTOs
{
    public class CategoryReadDto
    {
        public Guid CategoryId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}