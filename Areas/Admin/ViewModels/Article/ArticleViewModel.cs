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

namespace Blog.Areas.Admin.ViewModels.Article
{
    public class ArticleViewModel
    {
        public Guid? Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Content { get; set; }
        public IFormFile? ImageFile { get; set; }

        public string? Slug { set; get; }
        public string? Image { set; get; }

        public List<Guid>? ArticleCategories { get; set; } = new();

        public IEnumerable<SelectListItem>? ArticleCategoryList { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
