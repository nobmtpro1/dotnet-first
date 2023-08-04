using System;
using System.Collections.Generic;
using Blog.Areas.Admin.ViewModels.Article;
using Blog.Models;
using Microsoft.AspNetCore.Hosting;

namespace Blog.Services.Interfaces
{
    public interface IArticleRepository : IBaseRepository<ArticleModel>
    {
        public bool CheckSlugExist(string slug);
    }
}