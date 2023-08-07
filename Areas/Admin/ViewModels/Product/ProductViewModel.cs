using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Blog.Data;
using Blog.Models;
using Blog.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Areas.Admin.ViewModels.Product
{
    public class ProductViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string? Content { get; set; }
        public IFormFile? ImageFile { get; set; }

        [Required]
        public string Slug { set; get; } = default!;
        public string? Image { set; get; }

        public List<Guid>? ProductCategories { get; set; } = new();

        public IEnumerable<SelectListItem>? ProductCategoryList { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
