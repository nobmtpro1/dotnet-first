using System;
using System.Collections.Generic;
using Blog.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Services.Interfaces
{
    public interface IArticleCategoryRepository : IBaseRepository<ArticleCategoryModel>
    {
        public IEnumerable<SelectListItem> ToSelectListItems();

    }
}