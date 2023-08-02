using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Blog.Services.Interfaces;

namespace Blog.Models
{
    [Table("Article")]
    public class ArticleModel : BaseModel
    {
        public ArticleModel()
        {
            this.ArticleCategories = new HashSet<ArticleCategoryModel>();
        }
        public string Title { set; get; } = default!;

        public string? Content { set; get; }

        public string? Image { set; get; }

        public string Slug { set; get; } = default!;

        public virtual ICollection<ArticleCategoryModel> ArticleCategories { get; set; }
    }
}