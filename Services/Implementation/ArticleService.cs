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
                // Id = Guid.NewGuid(),
                Title = model.Title,
                Content = model.Content
            };
            _unitOfWork.ArticleRepository.Insert(article);
            _unitOfWork.SaveChanges();
            return article;
        }

        // public List<ProductCategoryViewModel> GetProductCategorys()
        // {
        //     _logger.LogDebug($"Get all product category");
        //     var productCategories = UnitOfWork.ProductCategoryRepo.GetAll().Where(x => x.IsActive).Select(x => new ProductCategoryViewModel()
        //     {
        //         Id = x.Id,
        //         ParentId = x.ParentId,
        //         Name = x.Name,
        //         DisplayOrder = x.DisplayOrder,
        //         Description = x.Description,
        //         IsActive = x.IsActive
        //     }).OrderBy(x => x.Name).ToList();

        //     return productCategories;
        // }

        // public ProductCategoryViewModel GetProductCategory(int id)
        // {
        //     _logger.LogDebug($"Detail service (Id: {id})");
        //     var entity = UnitOfWork.ProductCategoryRepo.GetById(id);
        //     if (entity != null)
        //     {
        //         var model = new ProductCategoryViewModel()
        //         {
        //             Id = entity.Id,
        //             ParentId = entity.ParentId,
        //             Name = entity.Name,
        //             DisplayOrder = entity.DisplayOrder,
        //             Description = entity.Description,
        //             IsActive = entity.IsActive
        //         };

        //         return model;
        //     }
        //     return null;
        // }
        // public void CreateProductCategory(ProductCategoryViewModel model)
        // {
        //     //_logger.LogDebug($"Create (Name: {model.Name})");
        //     var entity = new ProductCategory
        //     {
        //         Id = model.Id,
        //         Name = model.Name,
        //         ParentId = model.ParentId,
        //         DisplayOrder = model.DisplayOrder,
        //         Description = model.Description,
        //         IsActive = model.IsActive
        //     };

        //     UnitOfWork.ProductCategoryRepo.Insert(entity);
        //     UnitOfWork.SaveChanges();
        // }
        // public void UpdateProductCategory(ProductCategoryViewModel model)
        // {
        //     _logger.LogDebug($"Edit (Id: {model.Id})");
        //     var entity = UnitOfWork.ProductCategoryRepo.GetById(model.Id);
        //     if (entity != null)
        //     {
        //         entity.Name = model.Name;
        //         entity.ParentId = model.ParentId;
        //         entity.DisplayOrder = model.DisplayOrder;
        //         entity.Description = model.Description;
        //         entity.IsActive = model.IsActive;
        //         UnitOfWork.ProductCategoryRepo.Update(entity);
        //         UnitOfWork.SaveChanges();
        //     }
        // }
        // public void DeleteProductCategory(int id)
        // {
        //     _logger.LogDebug($"Delete (Id: {id})");
        //     var entity = UnitOfWork.ProductCategoryRepo.GetById(id);
        //     if (entity != null)
        //     {
        //         UnitOfWork.ProductCategoryRepo.Delete(entity);
        //         UnitOfWork.SaveChanges();
        //     }
        // }


    }
}
