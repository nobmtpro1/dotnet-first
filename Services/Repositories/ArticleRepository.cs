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

namespace Blog.Services.Repository
{
    public class ArticleRepository : BaseRepository<ArticleModel>, IArticleRepository
    {
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
    }
}