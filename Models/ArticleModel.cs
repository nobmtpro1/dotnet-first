using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Models
{
    public class ArticleModel
    {
        [Key]
        public int Id { set; get; }

        public string? Title { set; get; }

        public string? Content { set; get; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}