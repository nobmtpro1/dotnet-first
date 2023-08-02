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

namespace Blog.Areas.Admin.Controllers;

[Authorize(Roles = Const.ROLE_ADMIN)]
public class ArticleController : Controller
{
    private readonly ILogger<ArticleController> _logger;
    private IArticleService _articleService;
    private IArticleCategoryService _articleCategoryService;
    private IWebHostEnvironment _hostingEnv;

    public ArticleController(ILogger<ArticleController> logger, IArticleService articleService, IArticleCategoryService articleCategoryService, IWebHostEnvironment hostingEnv)
    {
        _logger = logger;
        _articleService = articleService;
        _articleCategoryService = articleCategoryService;
        _hostingEnv = hostingEnv;
    }

    public IActionResult Index()
    {
        ArticleListViewModel model = new ArticleListViewModel();
        var articles = _articleService.GetAll();
        model.Articles = articles;
        return View(model);
    }

    [HttpGet]
    public IActionResult Add()
    {
        ArticleViewModel model = new ArticleViewModel();
        model = _initArticleViewModel(model, null);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(ArticleViewModel model)
    {
        if (ModelState.IsValid)
        {
            model.Slug = Helper.GenerateSlug(model.Slug);
            if (_articleService.CheckSlugExist(model.Slug))
            {
                ModelState.AddModelError("Slug", "Slug is already existed");
                return View(model);
            }
            var article = _articleService.Insert(model);
            TempData["Message"] = "Create successfully";
            return RedirectToAction("Add");
        }
        model = _initArticleViewModel(model, null);
        return View(model);
    }

    [HttpGet]
    public IActionResult Edit(Guid Id)
    {
        var article = _articleService.GetById(Id);
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

        model = _initArticleViewModel(model, article);
        // Dumper.Dump(model);
        ViewData["imagePath"] = Helper.BaseUrl(Request) + Const.UPLOAD_IMAGE_DIR;
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(ArticleViewModel model, Guid Id)
    {
        var article = _articleService.GetById(Id);
        if (article == null)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            model.Slug = Helper.GenerateSlug(model.Slug);
            if (article.Slug != model.Slug && _articleService.CheckSlugExist(model.Slug))
            {
                ModelState.AddModelError("Slug", "Slug is already existed");
                model = _initArticleViewModel(model, article);
                return View(model);
            }
            _articleService.Update(Id, model);
            TempData["Message"] = "Update successfully";
            return RedirectToAction("Edit", new { Id = Id });
        }
        model = _initArticleViewModel(model, article);
        return View(model);
    }

    [HttpGet]
    public IActionResult Delete(Guid Id)
    {
        var article = _articleService.GetById(Id);
        if (article == null)
        {
            return NotFound();
        }
        _articleService.Delete(article);
        TempData["Message"] = "Delete successfully";
        return RedirectToAction("Index");
    }

    private ArticleViewModel _initArticleViewModel(ArticleViewModel model, ArticleModel? article)
    {
        var articleCategorySelectListItems = _articleCategoryService.GetArticleCategorySelectListItem();
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
