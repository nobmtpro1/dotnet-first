using System;
using System.ComponentModel.DataAnnotations;
using Blog.Services.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    [Table("ArticleCategory")]
    public class ArticleCategoryModel : BaseModel
    {

        public ArticleCategoryModel()
        {
            this.Articles = new HashSet<ArticleModel>();
        }
        public string? Name { set; get; }

        public string? Slug { set; get; }

        public Guid? ParentId { set; get; }

        public virtual  ArticleCategoryModel? Parent { get; set; } = null!;
        public virtual ICollection<ArticleCategoryModel> Children { get; set; } = new List<ArticleCategoryModel>();

        public virtual ICollection<ArticleModel> Articles { get; set; }
    }
}