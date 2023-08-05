using System.Collections.Generic;

namespace Blog.Areas.Admin.ViewModels.ProductCategory
{
    public class ProductCategoryListViewModel
    {
        public List<ProductCategoryViewModel> ProductCategories { get; set; } = default!;
    }
}
