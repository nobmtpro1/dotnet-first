using System;
using System.Collections.Generic;
using System.Text;
using Blog.Models;

namespace Blog.Services.Interfaces
{
    public interface IArticleRepository : IRepository<ArticleModel, int>
    {
    }
}
