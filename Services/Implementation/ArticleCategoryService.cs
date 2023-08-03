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
using Blog.Areas.Admin.ViewModels.ArticleCategory;

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

        public ArticleCategoryModel Insert(ArticleCategoryViewModel model)
        {
            var articleCategory = new ArticleCategoryModel()
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                ParentId = model.ParentId,
                Slug = model.Slug,
            };
            _unitOfWork.ArticleCategoryRepository.Insert(articleCategory);
            _unitOfWork.SaveChanges();
            return articleCategory;
        }

        public ArticleCategoryModel GetById(Guid Id)
        {
            var articleCategory = _unitOfWork.ArticleCategoryRepository.GetById(Id);
            return articleCategory;
        }
        public bool CheckSlugExist(string slug)
        {
            var isExist = _unitOfWork.ArticleCategoryRepository.GetAll().Where(x => x.Slug == slug).Any();
            return isExist;
        }

        public ArticleCategoryModel Update(Guid Id, ArticleCategoryViewModel model)
        {
            var articleCategory = GetById(Id);
            articleCategory.Name = model.Name;
            articleCategory.ParentId = model.ParentId;
            articleCategory.Slug = model.Slug;
            _unitOfWork.ArticleCategoryRepository.Update(articleCategory, x => x.Name!, x => x.ParentId!, x => x.Slug!);
            _unitOfWork.SaveChanges();
            return articleCategory;
        }

        public ArticleCategoryModel Delete(ArticleCategoryModel articleCategory)
        {
            _unitOfWork.ArticleCategoryRepository.Delete(articleCategory);
            _unitOfWork.SaveChanges();
            return articleCategory;
        }
    }
}
