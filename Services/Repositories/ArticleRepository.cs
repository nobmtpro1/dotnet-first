using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Blog.Models;
using Blog.Services.Interfaces;
using Blog.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Blog.Areas.Admin.ViewModels.Article;
using Blog.Ultils;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace Blog.Services.Repository
{
    public class ArticleRepository : BaseRepository<ArticleModel>, IArticleRepository
    {
        public int pageSize = 12;
        public ArticleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public bool CheckSlugExist(string slug)
        {
            var isExist = dbSet.Where(x => x.Slug == slug).Any();
            return isExist;
        }

        public ArticleModel? GetBySlug(string slug)
        {
            var article = dbSet.Where(x => x.Slug == slug).FirstOrDefault();
            return article;
        }

        public async Task<PaginatedList<ArticleModel>> Search(int? page, string? categorySlug, int? year)
        {
            var articles = DbSet();
            if (!string.IsNullOrEmpty(categorySlug))
            {
                articles = articles.Where(s => s.ArticleCategories.Where(x => x.Slug == categorySlug).Any());
            }
            if (year != null)
            {
                articles = articles.Where(x => x.CreatedAt.HasValue).Where(x => x.CreatedAt!.Value.Year.ToString() == year.ToString());
            }
            articles = articles.OrderByDescending(x => x.CreatedAt);
            var articlesPaginatedList = await PaginatedList<ArticleModel>.CreateAsync(articles, page ?? 1, pageSize);
            return articlesPaginatedList;
        }

        public List<int> SelectDistinctYear()
        {
            var distinctYear = dbSet.Select(s => s.CreatedAt!.Value.Year).Distinct().OrderBy(x => x).ToList();
            return distinctYear;
        }
    }
}