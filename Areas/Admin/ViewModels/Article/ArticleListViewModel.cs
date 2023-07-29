using System.Collections.Generic;

namespace Blog.Areas.Admin.ViewModels.Article
{
    public class ArticleListViewModel
    {
        public List<ArticleViewModel> Articles { get; set; } = null!;
    }
}
