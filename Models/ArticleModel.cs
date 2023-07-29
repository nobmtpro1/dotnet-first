using System;
using System.ComponentModel.DataAnnotations;
using Blog.Services.Interfaces;

namespace Blog.Models
{
    public class ArticleModel : IEntity<int>
    {
        [Key]
        public int Id { set; get; }

        public string? Title { set; get; }

        public string? Content { set; get; }

        [DisplayFormat(DataFormatString = "{dd/MM/yyyy}")]
        public DateTime? CreatedAt { get; set; }

        [DisplayFormat(DataFormatString = "{dd/MM/yyyy}")]
        public DateTime? UpdatedAt { get; set; }
    }
}