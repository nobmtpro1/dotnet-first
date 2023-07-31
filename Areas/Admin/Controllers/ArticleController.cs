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

namespace Blog.Areas.Admin.Controllers;

[Authorize(Roles = Constants.ROLE_ADMIN)]
public class ArticleController : Controller
{
    private readonly ILogger<ArticleController> _logger;
    private IArticleService _articleService;

    public ArticleController(ILogger<ArticleController> logger, IArticleService articleService)
    {
        _logger = logger;
        _articleService = articleService;
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
        return View();
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
}
