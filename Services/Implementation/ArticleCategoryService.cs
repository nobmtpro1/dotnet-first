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
    }
}
