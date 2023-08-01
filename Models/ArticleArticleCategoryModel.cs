using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models;

[Table("ArticleArticleCategory")]
public class ArticleArticleCategoryModel
{
    public Guid ArticleId { get; set; }
    public Guid ArticleCategoryId { get; set; }

    [ForeignKey("ArticleId")]
    public ArticleModel Article { get; set; } = new();

    [ForeignKey("ArticleCategoryId")]
    public ArticleCategoryModel ArticleCategory { get; set; } = new();
}