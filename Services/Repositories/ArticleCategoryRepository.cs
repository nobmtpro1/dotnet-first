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
    public class ArticleCategoryRepository : BaseRepository<ArticleCategoryModel>, IArticleCategoryRepository
    {
        public ArticleCategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<SelectListItem> ToSelectListItems()
        {
            var selectListItems = dbSet.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            return selectListItems;
        }
    }
}