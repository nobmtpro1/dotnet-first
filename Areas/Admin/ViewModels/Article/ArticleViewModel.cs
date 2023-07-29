using System;

namespace Blog.Areas.Admin.ViewModels.Article
{
    public class ArticleViewModel
    {
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
