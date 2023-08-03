using System;
using Blog.Models;
using Blog.Data;
using Blog.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Castle.Core.Configuration;

namespace Blog.Services
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private ApplicationDbContext context = default!;
        private GenericRepository<ArticleModel> articleRepository = default!;
        private GenericRepository<ArticleCategoryModel> articleCategoryRepository = default!;

        public UnitOfWork(ApplicationDbContext _context)
        {
            context = _context;
            // context = new ApplicationDbContext(options.Options);
            //     var contextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            //  .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Mentoring.Data;Trusted_Connection=True;MultipleActiveResultSets=true")
            //  .Options;
            //     context = new ApplicationDbContext(contextOptions);
        }

        public GenericRepository<ArticleModel> ArticleRepository
        {
            get
            {
                if (articleRepository == null)
                {
                    articleRepository = new GenericRepository<ArticleModel>(context);
                }
                return articleRepository;
            }
        }

        public GenericRepository<ArticleCategoryModel> ArticleCategoryRepository
        {
            get
            {
                if (articleCategoryRepository == null)
                {
                    articleCategoryRepository = new GenericRepository<ArticleCategoryModel>(context);
                }
                return articleCategoryRepository;
            }
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