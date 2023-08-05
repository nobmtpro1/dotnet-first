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

namespace Blog.Areas.Admin.ViewModels.ProductCategory
{
    public class ProductCategoryViewModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = default!;
        [Required]
        public string Slug { get; set; } = default!;
        public Guid? ParentId { get; set; } = default!;
        public IEnumerable<SelectListItem>? ParentList { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
