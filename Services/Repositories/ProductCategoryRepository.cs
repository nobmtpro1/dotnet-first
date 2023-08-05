using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Blog.Models;
using Blog.Services.Interfaces;
using Blog.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Services.Repository
{
    public class ProductCategoryRepository : BaseRepository<ProductCategoryModel>, IProductCategoryRepository
    {
        public ProductCategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override ProductCategoryModel Delete(ProductCategoryModel entityToDelete)
        {
            if (context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            foreach (var child in entityToDelete.Children)
            {
                child.ParentId = null;
            }
            dbSet.Remove(entityToDelete);
            return entityToDelete;
        }

        public IEnumerable<SelectListItem> ToSelectListItems()
        {
            var selectListItems = dbSet.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            return selectListItems;
        }

        public bool CheckSlugExist(string slug)
        {
            var isExist = dbSet.Where(x => x.Slug == slug).Any();
            return isExist;
        }
    }
}