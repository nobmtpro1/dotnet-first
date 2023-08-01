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

namespace Blog.Areas.Admin.Controllers;

[Authorize(Roles = Constants.ROLE_ADMIN)]
public class ArticleController : Controller
{
    private readonly ILogger<ArticleController> _logger;
    private IArticleService _articleService;
    private IArticleCategoryService _articleCategoryService;

    public ArticleController(ILogger<ArticleController> logger, IArticleService articleService, IArticleCategoryService articleCategoryService)
    {
        _logger = logger;
        _articleService = articleService;
        _articleCategoryService = articleCategoryService;
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
        var articleCategorySelectListItems = _articleCategoryService.GetArticleCategorySelectListItem();
        ArticleViewModel model = new ArticleViewModel();
        model.ArticleCategoryList = articleCategorySelectListItems;
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(ArticleViewModel model)
    {
        var article = _articleService.Insert(model);
        TempData["Message"] = "Create successfully";
        return RedirectToAction("Add");
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
            UpdatedAt = article.UpdatedAt != null ? article.UpdatedAt!.Value : null,
            CreatedAt = article.CreatedAt != null ? article.CreatedAt!.Value : null,
        };
        var articleCategorySelectListItems = _articleCategoryService.GetArticleCategorySelectListItem();
        model.ArticleCategoryList = articleCategorySelectListItems;
        var articleCategories = new List<Guid>() { };
        foreach (var item in article.ArticleCategories)
        {
            articleCategories.Add(item.Id);
        }
        model.ArticleCategories = articleCategories;
        
        // Dumper.Dump(model);
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
        _articleService.Update(Id, model);
        TempData["Message"] = "Update successfully";
        return RedirectToAction("Edit", new { Id = Id });
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
}
