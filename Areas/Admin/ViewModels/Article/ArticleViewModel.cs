using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Areas.Admin.ViewModels.Article
{
    public class ArticleViewModel
    {
        public Guid? Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
