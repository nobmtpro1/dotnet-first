using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Blog;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using Blog.Areas.Admin.ViewModels.ArticleCategory;
using Blog.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Blog.Ultils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Blog.Areas.Admin.Controllers;

[Authorize(Roles = Const.ROLE_ADMIN)]
public class ArticleCategoryController : Controller
{
    private readonly ILogger<ArticleCategoryController> _logger;
    private IArticleCategoryService _articleCategoryService;
    private IWebHostEnvironment _hostingEnv;

    public ArticleCategoryController(ILogger<ArticleCategoryController> logger, IArticleCategoryService articleCategoryService, IWebHostEnvironment hostingEnv)
    {
        _logger = logger;
        _articleCategoryService = articleCategoryService;
        _hostingEnv = hostingEnv;
    }

    public IActionResult Index()
    {
        ArticleCategoryListViewModel model = new ArticleCategoryListViewModel();
        var articleCategories = _articleCategoryService.GetAll();
        var articleCategoriesListView = new List<ArticleCategoryViewModel>();
        foreach (var item in articleCategories)
        {
            articleCategoriesListView.Add(new ArticleCategoryViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                CreatedAt = item.CreatedAt!.Value,
                UpdatedAt = item.UpdatedAt!.Value,
            });
        }
        model.ArticleCategories = articleCategoriesListView;
        return View(model);
    }


    [HttpGet]
    public IActionResult Add()
    {
        ArticleCategoryViewModel model = new ArticleCategoryViewModel();
        var articleCategories = _articleCategoryService.GetAll();
        var parentList = new List<SelectListItem>();
        foreach (var item in articleCategories)
        {
            parentList.Add(new SelectListItem()
            {
                Text = item.Name,
                Value = item.Id.ToString()
            });
        }
        model.ParentList = parentList;
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(ArticleCategoryViewModel model)
    {
        if (ModelState.IsValid)
        {
            _articleCategoryService.Insert(model);
            TempData["Message"] = "Create successfully";
            return RedirectToAction("Add");
        }

        return View(model);
    }
}
