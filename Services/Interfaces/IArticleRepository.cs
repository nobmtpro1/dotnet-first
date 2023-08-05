using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Areas.Admin.ViewModels.Article;
using Blog.Models;
using Blog.Ultils;
using Microsoft.AspNetCore.Hosting;

namespace Blog.Services.Interfaces
{
    public interface IArticleRepository : IBaseRepository<ArticleModel>
    {
        public bool CheckSlugExist(string slug);
        public ArticleModel? GetBySlug(string slug);
        public Task<PaginatedList<ArticleModel>> Search(int? page, string? categorySlug, int? year);
        public List<int> SelectDistinctYear();
    }
}