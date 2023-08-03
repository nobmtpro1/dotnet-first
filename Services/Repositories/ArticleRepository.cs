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
    public class ArticleRepository : IArticleRepository, IDisposable
    {
        private readonly ApplicationDbContext context;

        public ArticleRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<ArticleModel> GetArticles()
        {
            return context.Article.ToList();
        }

        public ArticleModel GetArticleByID(int id)
        {
            return context.Article.Find(id)!;
        }

        public void InsertArticle(ArticleModel article)
        {
            context.Article.Add(article);
        }

        public void DeleteArticle(int articleID)
        {
            ArticleModel article = context.Article.Find(articleID!)!;
            context.Article.Remove(article!);
        }

        public void UpdateArticle(ArticleModel article)
        {
            context.Entry(article).State = EntityState.Modified;
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