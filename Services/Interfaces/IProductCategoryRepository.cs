using System;
using System.Collections.Generic;
using Blog.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Services.Interfaces
{
    public interface IProductCategoryRepository : IBaseRepository<ProductCategoryModel>
    {
        public IEnumerable<SelectListItem> ToSelectListItems();
        public bool CheckSlugExist(string slug);

    }
}