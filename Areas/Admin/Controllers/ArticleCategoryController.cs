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
using System.Linq;

namespace Blog.Areas.Admin.Controllers;

[Authorize(Roles = Const.ROLE_ADMIN)]
[Area("Admin")]
public class ArticleCategoryController : Controller
{
    private readonly ILogger<ArticleController> _logger;
    private readonly IWebHostEnvironment _hostingEnv;
    private readonly IUnitOfWork _unitOfWork;

    public ArticleCategoryController(ILogger<ArticleController> logger, IWebHostEnvironment hostingEnv, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _hostingEnv = hostingEnv;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        ArticleCategoryListViewModel model = new();
        var articleCategories = _unitOfWork.ArticleCategoryRepository.Get(orderBy: q => q.OrderBy(d => d.CreatedAt));
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
        ArticleCategoryViewModel model = new();
        model = InitArticleCategoryViewModel(model);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(ArticleCategoryViewModel model)
    {
        if (ModelState.IsValid)
        {
            var articleCategory = new ArticleCategoryModel()
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                ParentId = model.ParentId,
                Slug = model.Slug,
            };
            _unitOfWork.ArticleCategoryRepository.Insert(articleCategory);
            _unitOfWork.Save();
            TempData["Message"] = "Create successfully";
            return RedirectToAction("Add");
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Edit(Guid Id)
    {
        var articleCategory = _unitOfWork.ArticleCategoryRepository.GetByID(Id);
        // Dumper.Dump(article.ArticleCategories);
        if (articleCategory == null)
        {
            return NotFound();
        }
        var model = new ArticleCategoryViewModel()
        {
            Id = articleCategory.Id,
            Name = articleCategory.Name,
            Slug = articleCategory.Slug,
            ParentId = articleCategory.ParentId,
            UpdatedAt = articleCategory.UpdatedAt,
            CreatedAt = articleCategory.CreatedAt,
        };

        model = InitArticleCategoryViewModel(model, Id);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(ArticleCategoryViewModel model, Guid Id)
    {
        var articleCategory = _unitOfWork.ArticleCategoryRepository.GetByID(Id);
        if (articleCategory == null)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            model.Slug = Helper.GenerateSlug(model.Slug);
            if (articleCategory.Slug != model.Slug && _unitOfWork.ArticleCategoryRepository.CheckSlugExist(model.Slug))
            {
                ModelState.AddModelError("Slug", "Slug is already existed");
                model = InitArticleCategoryViewModel(model, Id);
                return View(model);
            }
            articleCategory.Name = model.Name;
            articleCategory.ParentId = model.ParentId;
            articleCategory.Slug = model.Slug;
            _unitOfWork.ArticleCategoryRepository.Update(articleCategory);
            _unitOfWork.Save();
            TempData["Message"] = "Update successfully";
            return RedirectToAction("Edit", new { Id });
        }
        model = InitArticleCategoryViewModel(model, Id);
        return View(model);
    }

    [HttpGet]
    public IActionResult Delete(Guid Id)
    {
        var articleCategory = _unitOfWork.ArticleCategoryRepository.GetByID(Id);
        if (articleCategory == null)
        {
            return NotFound();
        }
        _unitOfWork.ArticleCategoryRepository.Delete(articleCategory);
        _unitOfWork.Save();
        TempData["Message"] = "Delete successfully";
        return RedirectToAction("Index");
    }

    private ArticleCategoryViewModel InitArticleCategoryViewModel(ArticleCategoryViewModel model, Guid? excludeId = null)
    {
        var articleCategories = _unitOfWork.ArticleCategoryRepository.Get(orderBy: q => q.OrderBy(d => d.CreatedAt));
        var parentList = new List<SelectListItem>();

        foreach (var item in articleCategories)
        {
            if (excludeId != null && item.Id == excludeId)
            {
                continue;
            }
            parentList.Add(new SelectListItem()
            {
                Text = item.Name,
                Value = item.Id.ToString()
            });
        }
        model.ParentList = parentList;
        return model;
    }
}
