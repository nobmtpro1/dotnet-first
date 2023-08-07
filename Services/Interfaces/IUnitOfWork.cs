
using Blog.Models;

namespace Blog.Services.Interfaces;
public interface IUnitOfWork
{
    public IArticleRepository ArticleRepository { get; }
    public IArticleCategoryRepository ArticleCategoryRepository { get; }
    public IProductCategoryRepository ProductCategoryRepository { get; }
    public IProductRepository ProductRepository { get; }
    public void Save();
    public void Dispose();
}