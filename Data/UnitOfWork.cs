using System;
using System.Threading.Tasks;
using Blog.Data.Repositories;
using Blog.Services.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Blog.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private Guid _userId;
        private ApplicationDbContext _dbContext;
        private IDbContextTransaction transaction = null!;

        public UnitOfWork(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public void SetUser(Guid userId)
        {
            this._userId = userId;
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }
        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void BeginTransaction()
        {
            transaction = _dbContext.Database.BeginTransaction();
        }

        public void CommitTransaction()
        {
            transaction.Commit();
        }

        public void RollbackTransaction()
        {
            transaction.Rollback();
        }

        private IArticleRepository _articleRepository = null!;
        public IArticleRepository ArticleRepository => _articleRepository ?? (_articleRepository = new ArticleRepository(_dbContext));

        private IArticleCategoryRepository _articleCategoryRepository = null!;
        public IArticleCategoryRepository ArticleCategoryRepository => _articleCategoryRepository ?? (_articleCategoryRepository = new ArticleCategoryRepository(_dbContext));

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (_dbContext == null) return;
            _dbContext.Dispose();
        }
    }
}

