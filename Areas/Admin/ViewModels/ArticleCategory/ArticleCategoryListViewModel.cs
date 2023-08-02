using System.Collections.Generic;

namespace Blog.Areas.Admin.ViewModels.ArticleCategory
{
    public class ArticleCategoryListViewModel
    {
        public List<ArticleCategoryViewModel> ArticleCategories { get; set; } = default!;
    }
}
