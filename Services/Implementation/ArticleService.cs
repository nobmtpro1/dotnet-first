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
using Microsoft.AspNetCore.Hosting;
using Blog;
using System.Threading.Tasks;

namespace Blog.Services.Implementation
{
    public class ArticleService : IArticleService
    {
        private readonly ILogger<ArticleService> _logger;
        private IConfiguration _config;
        private IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _hostingEnv;

        public ArticleService(ILogger<ArticleService> logger, IConfiguration config, IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnv)
        {
            _logger = logger;
            _config = config;
            _unitOfWork = unitOfWork;
            _hostingEnv = hostingEnv;
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
                Slug = model.Slug,
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

            if (model.ImageFile != null)
            {
                var fileName = Helper.UploadFile(model.ImageFile, _hostingEnv.WebRootPath, Const.UPLOAD_IMAGE_DIR);
                article.Image = fileName;
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
            article.Slug = model.Slug;
            _unitOfWork.ArticleRepository.Update(article, x => x.Title!, x => x.Content!, x => x.Slug!);
            foreach (var item in article.ArticleCategories)
            {
                article.ArticleCategories.Remove(item);
            }
            if (model.ArticleCategories != null)
            {
                var articleCategories = _unitOfWork.ArticleCategoryRepository.GetAll().Where(x => model.ArticleCategories.Contains(x.Id)).ToList();
                foreach (var item in articleCategories)
                {
                    article.ArticleCategories.Add(item);
                }
            }
            if (model.ImageFile != null)
            {
                var fileName = Helper.UploadFile(model.ImageFile, _hostingEnv.WebRootPath, Const.UPLOAD_IMAGE_DIR);
                article.Image = fileName;
            }
            _unitOfWork.SaveChanges();

            return article;
        }

        public ArticleModel Delete(ArticleModel article)
        {
            _unitOfWork.ArticleRepository.Delete(article);
            _unitOfWork.SaveChanges();
            return article;
        }

        public bool CheckSlugExist(string slug)
        {
            var isExist = _unitOfWork.ArticleRepository.GetAll().Where(x => x.Slug == slug).Any();
            return isExist;
        }

        public async Task<PaginatedList<ArticleModel>> Search(int pageIndex)
        {
            var articles = _unitOfWork.ArticleRepository.GetAll().OrderByDescending(x => x.CreatedAt);
            PaginatedList<ArticleModel> articlesPaginatedList = await PaginatedList<ArticleModel>.CreateAsync(articles, pageIndex, 2);
            return articlesPaginatedList;
        }
    }
}
