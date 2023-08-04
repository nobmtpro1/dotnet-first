using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Blog;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using Blog.Areas.Admin.ViewModels.Article;
using Blog.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Blog.Ultils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Data.SqlClient;
using Blog.Services;
using System.Linq;

namespace Blog.Areas.Admin.Controllers;

[Authorize(Roles = Const.ROLE_ADMIN)]
[Area("Admin")]
public class ArticleController : Controller
{
    private readonly ILogger<ArticleController> _logger;
    private readonly IWebHostEnvironment _hostingEnv;
    private readonly IUnitOfWork _unitOfWork;

    public ArticleController(ILogger<ArticleController> logger, IWebHostEnvironment hostingEnv, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _hostingEnv = hostingEnv;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        ArticleListViewModel model = new();
        var articles = _unitOfWork.ArticleRepository.Get(orderBy: q => q.OrderBy(d => d.CreatedAt));
        var articlesList = new List<ArticleViewModel>();
        foreach (var article in articles)
        {
            articlesList.Add(new ArticleViewModel()
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                Image = article.Image,
                Slug = article.Slug,
                UpdatedAt = article.UpdatedAt != null ? article.UpdatedAt!.Value : null,
                CreatedAt = article.CreatedAt != null ? article.CreatedAt!.Value : null,
            });
        }
        model.Articles = articlesList;
        return View(model);
    }

    [HttpGet]
    public IActionResult Add()
    {
        ArticleViewModel model = new();
        model = InitArticleViewModel(model, null);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(ArticleViewModel model)
    {
        if (ModelState.IsValid)
        {
            model.Slug = Helper.GenerateSlug(model.Slug);
            if (_unitOfWork.ArticleRepository.CheckSlugExist(model.Slug))
            {
                ModelState.AddModelError("Slug", "Slug is already existed");
                return View(model);
            }

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
                    var articleCategory = _unitOfWork.ArticleCategoryRepository.GetByID(item);
                    article.ArticleCategories.Add(articleCategory);
                }
            }

            if (model.ImageFile != null)
            {
                var fileName = Helper.UploadFile(model.ImageFile, _hostingEnv.WebRootPath, Const.UPLOAD_IMAGE_DIR);
                article.Image = fileName;
            }

            _unitOfWork.ArticleRepository.Insert(article);
            _unitOfWork.Save();
            TempData["Message"] = "Create successfully";
            return RedirectToAction("Add");
        }
        model = InitArticleViewModel(model, null);
        return View(model);
    }

    [HttpGet]
    public IActionResult Edit(Guid Id)
    {
        var article = _unitOfWork.ArticleRepository.GetByID(Id);
        // Dumper.Dump(article.ArticleCategories);
        if (article == null)
        {
            return NotFound();
        }
        var model = new ArticleViewModel()
        {
            Id = article.Id,
            Title = article.Title,
            Content = article.Content,
            Image = article.Image,
            Slug = article.Slug,
            UpdatedAt = article.UpdatedAt != null ? article.UpdatedAt!.Value : null,
            CreatedAt = article.CreatedAt != null ? article.CreatedAt!.Value : null,
        };

        model = InitArticleViewModel(model, article);
        // Dumper.Dump(model);
        ViewData["imagePath"] = Helper.BaseUrl(Request) + Const.UPLOAD_IMAGE_DIR;
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(ArticleViewModel model, Guid Id)
    {
        var article = _unitOfWork.ArticleRepository.GetByID(Id);
        if (article == null)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            model.Slug = Helper.GenerateSlug(model.Slug);
            if (article.Slug != model.Slug && _unitOfWork.ArticleRepository.CheckSlugExist(model.Slug))
            {
                ModelState.AddModelError("Slug", "Slug is already existed");
                model = InitArticleViewModel(model, article);
                return View(model);
            }

            article.Title = model.Title;
            article.Content = model.Content;
            article.Slug = model.Slug;
            foreach (var item in article.ArticleCategories)
            {
                article.ArticleCategories.Remove(item);
            }
            if (model.ArticleCategories != null)
            {
                var articleCategories = _unitOfWork.ArticleCategoryRepository.Get(x => model.ArticleCategories.Contains(x.Id));
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

            _unitOfWork.ArticleRepository.Update(article);
            _unitOfWork.Save();
            TempData["Message"] = "Update successfully";
            return RedirectToAction("Edit", new { Id });
        }
        model = InitArticleViewModel(model, article);
        return View(model);
    }

    [HttpGet]
    public IActionResult Delete(Guid Id)
    {
        var article = _unitOfWork.ArticleRepository.GetByID(Id);
        if (article == null)
        {
            return NotFound();
        }
        _unitOfWork.ArticleRepository.Delete(article);
        _unitOfWork.Save();
        TempData["Message"] = "Delete successfully";
        return RedirectToAction("Index");
    }

    private ArticleViewModel InitArticleViewModel(ArticleViewModel model, ArticleModel? article)
    {
        var articleCategorySelectListItems = _unitOfWork.ArticleCategoryRepository.ToSelectListItems();
        model.ArticleCategoryList = articleCategorySelectListItems;

        if (article != null)
        {
            var articleCategories = new List<Guid>() { };
            foreach (var item in article.ArticleCategories)
            {
                articleCategories.Add(item.Id);
            }
            model.ArticleCategories = articleCategories;
        }

        return model;
    }
}
