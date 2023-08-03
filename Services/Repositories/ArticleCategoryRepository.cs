using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Blog.Models;
using Blog.Services.Interfaces;
using Blog.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Services.Repository
{
    public class ArticleCategoryRepository : IArticleCategoryRepository, IDisposable
    {
        private ApplicationDbContext context;

        public ArticleCategoryRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<ArticleCategoryModel> GetArticleCategories()
        {
            return context.ArticleCategory.ToList();
        }

        public ArticleCategoryModel GetArticleCategoryByID(int id)
        {
            return context.ArticleCategory.Find(id!)!;
        }

        public void InsertArticleCategory(ArticleCategoryModel articleCategory)
        {
            context.ArticleCategory.Add(articleCategory);
        }

        public void DeleteArticleCategory(int articleCategoryID)
        {
            ArticleCategoryModel articleCategory = context.ArticleCategory.Find(articleCategoryID!)!;
            context.ArticleCategory.Remove(articleCategory!);
        }

        public void UpdateArticleCategory(ArticleCategoryModel articleCategory)
        {
            context.Entry(articleCategory).State = EntityState.Modified;
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}