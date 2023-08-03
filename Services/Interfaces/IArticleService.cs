using System;
using System.Collections.Generic;
using System.Text;
using Blog.Models;
using Blog.Areas.Admin.ViewModels.Article;
using Blog.Ultils;
using System.Threading.Tasks;

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
        public Task<PaginatedList<ArticleModel>> Search(int pageIndex);

    }
}
