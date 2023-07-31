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
    public class ArticleService : IArticleService
    {
        private readonly ILogger<ArticleService> _logger;
        private IConfiguration _config;
        private IUnitOfWork _unitOfWork;

        public ArticleService(ILogger<ArticleService> logger, IConfiguration config, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _config = config;
            _unitOfWork = unitOfWork;
        }

        public List<ArticleViewModel> GetAll()
        {
            var query = _unitOfWork.ArticleRepository.GetAll();
            var data = query.Select(x => new ArticleViewModel()
            {
                Id = x.Id!,
                Title = x.Title!,
                Content = x.Content!,
                CreatedAt = x.CreatedAt!.Value,
                UpdatedAt = x.UpdatedAt!.Value,
            }).ToList();

            return data;
        }

        public ArticleModel Insert(ArticleViewModel model)
        {
            var article = new ArticleModel()
            {
                Id = Guid.NewGuid(),
                Title = model.Title,
                Content = model.Content,
            };
            if (model.ArticleCategories != null)
            {
                foreach (var item in model.ArticleCategories)
                {
                    // article.ArticleCategories.Add(item);
                    var articleCategory = _unitOfWork.ArticleCategoryRepository.GetById(item);
                    article.ArticleCategories.Add(articleCategory);
                }
            }

            _unitOfWork.ArticleRepository.Insert(article);
            _unitOfWork.SaveChanges();
            return article;
        }

        public ArticleModel GetById(Guid Id)
        {
            var article = _unitOfWork.ArticleRepository.GetById(Id);
            return article;
        }

        public ArticleModel Update(Guid Id, ArticleViewModel model)
        {
            var article = GetById(Id);
            article.Title = model.Title;
            article.Content = model.Content;
            _unitOfWork.ArticleRepository.Update(article, x => x.Title!, x => x.Content!);
            _unitOfWork.SaveChanges();

            return article;
        }

        public ArticleModel Delete(ArticleModel article)
        {
            _unitOfWork.ArticleRepository.Delete(article);
            _unitOfWork.SaveChanges();
            return article;
        }
    }
}
