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
        public string? Name { set; get; }

        public string? Slug { set; get; }

        public Guid? ParentId { set; get; } = null!;

        [ForeignKey("ParentId")]
        public ArticleCategoryModel Parent { get; set; } = null!;
        public ICollection<ArticleCategoryModel> Children { get; set; } = null!;

        public List<ArticleModel> Articles { get; } = new();
    }
}