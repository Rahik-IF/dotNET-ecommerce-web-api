using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace web_api.DTOs
{
    public class CategoryUpdateDTO
    {
        [StringLength(100, MinimumLength =2, ErrorMessage ="Category name must be between 2 and 100 characters")]

        public  string Name { get; set; } = string.Empty;
        [StringLength(500, MinimumLength =2, ErrorMessage ="Category description can not exceed 500 characters")]
        public string? Description { get; set; } = string.Empty;
    }
}