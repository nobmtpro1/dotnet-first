using System;
using Blog.Models;
using Blog.Data;
using Blog.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Castle.Core.Configuration;
using Blog.Services.Repository;

namespace Blog.Services
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private readonly ApplicationDbContext context = default!;
        private IArticleRepository articleRepository = default!;
        private IArticleCategoryRepository articleCategoryRepository = default!;
        private IProductCategoryRepository productCategoryRepository = default!;
        private IProductRepository productRepository = default!;

        public UnitOfWork(ApplicationDbContext _context)
        {
            context = _context;
        }

        public IArticleRepository ArticleRepository
        {
            get
            {
                articleRepository ??= new ArticleRepository(context);
                return articleRepository;
            }
        }

        public IArticleCategoryRepository ArticleCategoryRepository
        {
            get
            {
                articleCategoryRepository ??= new ArticleCategoryRepository(context);
                return articleCategoryRepository;
            }
        }

        public IProductCategoryRepository ProductCategoryRepository
        {
            get
            {
                productCategoryRepository ??= new ProductCategoryRepository(context);
                return productCategoryRepository;
            }
        }

        public IProductRepository ProductRepository
        {
            get
            {
                productRepository ??= new ProductRepository(context);
                return productRepository;
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