using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Areas.Admin.ViewModels.Article
{
    public class ArticleViewModel
    {
        [Required]
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
