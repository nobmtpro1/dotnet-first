using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Blog.Models;
using Blog.Services.Interfaces;
using Blog.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Blog.Ultils;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace Blog.Services.Repository
{
    public class ProductRepository : BaseRepository<ProductModel>, IProductRepository
    {
        public int pageSize = 12;
        public ProductRepository(ApplicationDbContext context) : base(context)
        {
        }

        public bool CheckSlugExist(string slug)
        {
            var isExist = dbSet.Where(x => x.Slug == slug).Any();
            return isExist;
        }

        public ProductModel? GetBySlug(string slug)
        {
            var article = dbSet.Where(x => x.Slug == slug).FirstOrDefault();
            return article;
        }
    }
}