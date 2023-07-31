using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Blog.Models;

namespace Blog.Areas.Admin.ViewModels.Article
{
    public class ArticleViewModel
    {
        public Guid? Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Image { set; get; }

        public string? Slug { set; get; }

        public List<ArticleCategoryModel> ArticleCategories { get; } = new();
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
