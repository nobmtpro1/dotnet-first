using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Models;
using Blog.Ultils;
using Microsoft.AspNetCore.Hosting;

namespace Blog.Services.Interfaces
{
    public interface IProductRepository : IBaseRepository<ProductModel>
    {
        public bool CheckSlugExist(string slug);
        public ProductModel? GetBySlug(string slug);
    }
}