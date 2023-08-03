
using Blog.Models;

namespace Blog.Services.Interfaces;
public interface IUnitOfWork
{
    public GenericRepository<ArticleModel> ArticleRepository { get; }
    public GenericRepository<ArticleCategoryModel> ArticleCategoryRepository { get; }
    public void Save();
    public void Dispose();
}