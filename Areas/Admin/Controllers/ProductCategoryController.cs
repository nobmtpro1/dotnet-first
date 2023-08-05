using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Blog;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using Blog.Areas.Admin.ViewModels.ProductCategory;
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
public class ProductCategoryController : Controller
{
    private readonly ILogger<ProductCategoryController> _logger;
    private readonly IWebHostEnvironment _hostingEnv;
    private readonly IUnitOfWork _unitOfWork;

    public ProductCategoryController(ILogger<ProductCategoryController> logger, IWebHostEnvironment hostingEnv, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _hostingEnv = hostingEnv;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        ProductCategoryListViewModel model = new();
        var articleCategories = _unitOfWork.ProductCategoryRepository.Get(orderBy: q => q.OrderBy(d => d.CreatedAt));
        var articleCategoriesListView = new List<ProductCategoryViewModel>();
        foreach (var item in articleCategories)
        {
            articleCategoriesListView.Add(new ProductCategoryViewModel()
            {
                Id = item.Id,
                Name = item.Name,
                CreatedAt = item.CreatedAt!.Value,
                UpdatedAt = item.UpdatedAt!.Value,
            });
        }
        model.ProductCategories = articleCategoriesListView;
        ViewData["menuActive"] = Const.ADMIN_MENU_PRODUCT_CATEGORY;
        return View(model);
    }


    [HttpGet]
    public IActionResult Add()
    {
        ProductCategoryViewModel model = new();
        model = InitProductCategoryViewModel(model);
        ViewData["menuActive"] = Const.ADMIN_MENU_PRODUCT_CATEGORY;
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(ProductCategoryViewModel model)
    {
        if (ModelState.IsValid)
        {
            var articleCategory = new ProductCategoryModel()
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                ParentId = model.ParentId,
                Slug = model.Slug,
            };
            _unitOfWork.ProductCategoryRepository.Insert(articleCategory);
            _unitOfWork.Save();
            TempData["Message"] = "Create successfully";
            return RedirectToAction("Add");
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult Edit(Guid Id)
    {
        var articleCategory = _unitOfWork.ProductCategoryRepository.GetByID(Id);
        // Dumper.Dump(article.ProductCategories);
        if (articleCategory == null)
        {
            return NotFound();
        }
        var model = new ProductCategoryViewModel()
        {
            Id = articleCategory.Id,
            Name = articleCategory.Name,
            Slug = articleCategory.Slug,
            ParentId = articleCategory.ParentId,
            UpdatedAt = articleCategory.UpdatedAt,
            CreatedAt = articleCategory.CreatedAt,
        };

        model = InitProductCategoryViewModel(model, Id);
        ViewData["menuActive"] = Const.ADMIN_MENU_PRODUCT_CATEGORY;
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(ProductCategoryViewModel model, Guid Id)
    {
        var articleCategory = _unitOfWork.ProductCategoryRepository.GetByID(Id);
        if (articleCategory == null)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            model.Slug = Helper.GenerateSlug(model.Slug);
            if (articleCategory.Slug != model.Slug && _unitOfWork.ProductCategoryRepository.CheckSlugExist(model.Slug))
            {
                ModelState.AddModelError("Slug", "Slug is already existed");
                model = InitProductCategoryViewModel(model, Id);
                return View(model);
            }
            articleCategory.Name = model.Name;
            articleCategory.ParentId = model.ParentId;
            articleCategory.Slug = model.Slug;
            _unitOfWork.ProductCategoryRepository.Update(articleCategory);
            _unitOfWork.Save();
            TempData["Message"] = "Update successfully";
            return RedirectToAction("Edit", new { Id });
        }
        model = InitProductCategoryViewModel(model, Id);
        return View(model);
    }

    [HttpGet]
    public IActionResult Delete(Guid Id)
    {
        var articleCategory = _unitOfWork.ProductCategoryRepository.GetByID(Id);
        if (articleCategory == null)
        {
            return NotFound();
        }
        _unitOfWork.ProductCategoryRepository.Delete(articleCategory);
        _unitOfWork.Save();
        TempData["Message"] = "Delete successfully";
        return RedirectToAction("Index");
    }

    private ProductCategoryViewModel InitProductCategoryViewModel(ProductCategoryViewModel model, Guid? excludeId = null)
    {
        var articleCategories = _unitOfWork.ProductCategoryRepository.Get(orderBy: q => q.OrderBy(d => d.CreatedAt));
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
