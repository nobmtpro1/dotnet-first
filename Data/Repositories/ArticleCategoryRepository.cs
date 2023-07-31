using System;
using System.Collections.Generic;
using System.Text;
using Blog.Models;
using Blog.Services.Interfaces;
using Blog.Data;
using System.Linq;

namespace Blog.Data.Repositories
{
    public class ArticleCategoryRepository : Repository<ArticleCategoryModel, Guid>, IArticleCategoryRepository
    {
        public ArticleCategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
