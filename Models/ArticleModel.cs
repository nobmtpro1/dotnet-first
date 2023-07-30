using System;
using System.ComponentModel.DataAnnotations;
using Blog.Services.Interfaces;

namespace Blog.Models
{
    public class ArticleModel : BaseModel
    {
        public string? Title { set; get; }

        public string? Content { set; get; }
    }
}