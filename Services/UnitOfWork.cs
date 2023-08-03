using System;
using Blog.Models;
using Blog.Data;

namespace Blog.Services
{
    public class UnitOfWork : IDisposable
    {
        private ApplicationDbContext context = default!;
        private GenericRepository<ArticleModel> articleRepository = default!;
        private GenericRepository<ArticleCategoryModel> articleCategoryRepository = default!;

        public UnitOfWork(ApplicationDbContext _context){
            context  = _context!;
        }

        public GenericRepository<ArticleModel> ArticleRepository
        {
            get
            {
                if (this.articleRepository == null)
                {
                    this.articleRepository = new GenericRepository<ArticleModel>(context);
                }
                return articleRepository;
            }
        }

        public GenericRepository<ArticleCategoryModel> ArticleCategoryRepository
        {
            get
            {
                if (this.articleCategoryRepository == null)
                {
                    this.articleCategoryRepository = new GenericRepository<ArticleCategoryModel>(context);
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