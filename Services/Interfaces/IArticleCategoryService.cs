using System;
using System.Collections.Generic;
using System.Text;
using Blog.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Services.Interfaces
{
    public interface IArticleCategoryService
    {
        public List<ArticleCategoryModel> GetAll();
        public IEnumerable<SelectListItem> GetArticleCategorySelectListItem();
        public IEnumerable<SelectListItem> GetArticleCategorySelectListItemWithSelected(Guid Id);
    }
}
