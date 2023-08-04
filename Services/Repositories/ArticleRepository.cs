using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Blog.Models;
using Blog.Services.Interfaces;
using Blog.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Services.Repository
{
    public class ArticleRepository : BaseRepository<ArticleModel>, IArticleRepository
    {

        public ArticleRepository(ApplicationDbContext context) : base(context)
        {

        }
    }
}