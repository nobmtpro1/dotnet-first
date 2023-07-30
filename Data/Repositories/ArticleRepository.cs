using System;
using System.Collections.Generic;
using System.Text;
using Blog.Models;
using Blog.Services.Interfaces;
using Blog.Data;
using System.Linq;

namespace Blog.Data.Repositories
{
    public class ArticleRepository : Repository<ArticleModel, Guid>, IArticleRepository
    {
        public ArticleRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
