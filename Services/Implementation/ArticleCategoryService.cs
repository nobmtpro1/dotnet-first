using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Blog.Services.Interfaces;
using Blog.Areas.Admin.ViewModels.Article;
using Microsoft.Extensions.Logging;
using Blog.Models;
using Blog.Ultils;

namespace Blog.Services.Implementation
{
    public class ArticleCategoryService : IArticleCategoryService
    {
        private readonly ILogger<ArticleCategoryService> _logger;
        private IConfiguration _config;
        private IUnitOfWork _unitOfWork;

        public ArticleCategoryService(ILogger<ArticleCategoryService> logger, IConfiguration config, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _config = config;
            _unitOfWork = unitOfWork;
        }

        public List<ArticleCategoryModel> GetAll()
        {
            var query = _unitOfWork.ArticleCategoryRepository.GetAll().ToList();
            return query;
        }
        public IEnumerable<SelectListItem> GetArticleCategorySelectListItem()
        {
            var query = _unitOfWork.ArticleCategoryRepository.GetAll().Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Name });
            return query;
        }

        public IEnumerable<SelectListItem> GetArticleCategorySelectListItemWithSelected(Guid Id)
        {
            var query = GetArticleCategorySelectListItem().ToList();
            var queryArticle = _unitOfWork.ArticleRepository.GetAll();
            for (int i = 0; i < query.Count; i++)
            {
                var checkSelected = queryArticle.Where(p => p.ArticleCategories.Any(c => String.Equals(c.Id, query[i].Value))).Any();
                
                if (checkSelected == true)
                {
                    query[i].Selected = true;
                }
                query[i].Selected = true;
            }
            return query;
        }
    }
}
