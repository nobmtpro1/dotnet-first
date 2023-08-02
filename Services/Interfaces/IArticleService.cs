using System;
using System.Collections.Generic;
using System.Text;
using Blog.Models;
using Blog.Areas.Admin.ViewModels.Article;

namespace Blog.Services.Interfaces
{
    public interface IArticleService
    {
        public List<ArticleViewModel> GetAll();
        public ArticleModel Insert(ArticleViewModel model);
        public ArticleModel GetById(Guid Id);
        public ArticleModel Update(Guid Id, ArticleViewModel model);
        public ArticleModel Delete(ArticleModel article);
        public bool CheckSlugExist(string slug);
        // ProductCategoryViewModel GetProductCategory(int id);
        // void CreateProductCategory(ProductCategoryViewModel model);
        // void UpdateProductCategory(ProductCategoryViewModel model);
        // void DeleteProductCategory(int id);
        // List<ProductCategoryViewModel> GetProductCategorys();
    }
}
