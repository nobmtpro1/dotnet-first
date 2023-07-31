using System;
using System.Threading.Tasks;

namespace Blog.Services.Interfaces
{
    public interface IUnitOfWork
    {
        void SetUser(Guid userId);
        int SaveChanges();

        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        Task SaveChangesAsync();

        IArticleRepository ArticleRepository { get; }
        IArticleCategoryRepository ArticleCategoryRepository { get; }

    }
}
