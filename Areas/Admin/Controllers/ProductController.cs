using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Blog;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using Blog.Areas.Admin.ViewModels.Product;
using Blog.Services.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Blog.Ultils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.Data.SqlClient;
using Blog.Services;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Blog.Areas.Admin.Controllers;

[Authorize(Roles = Const.ROLE_ADMIN)]
[Area("Admin")]
public class ProductController : Controller
{
    private readonly ILogger<ProductController> _logger;
    private readonly IWebHostEnvironment _hostingEnv;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStorage _storage;
    private readonly IConfiguration _config;

    public ProductController(ILogger<ProductController> logger, IWebHostEnvironment hostingEnv, IUnitOfWork unitOfWork, IStorage storage, IConfiguration config)
    {
        _logger = logger;
        _hostingEnv = hostingEnv;
        _unitOfWork = unitOfWork;
        _storage = storage;
        _config = config;
    }

    public IActionResult Index()
    {
        ProductListViewModel model = new();
        var products = _unitOfWork.ProductRepository.Get(orderBy: q => q.OrderBy(d => d.CreatedAt));
        var productsList = new List<ProductViewModel>();
        foreach (var product in products)
        {
            productsList.Add(new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Content = product.Content,
                Description = product.Description,
                Image = product.Image,
                Slug = product.Slug,
                UpdatedAt = product.UpdatedAt != null ? product.UpdatedAt!.Value : null,
                CreatedAt = product.CreatedAt != null ? product.CreatedAt!.Value : null,
            });
        }
        model.Products = productsList;
        ViewData["menuActive"] = Const.ADMIN_MENU_PRODUCT;
        return View(model);
    }

    [HttpGet]
    public IActionResult Add()
    {
        ProductViewModel model = new();
        model = InitProductViewModel(model, null);
        ViewData["menuActive"] = Const.ADMIN_MENU_PRODUCT;
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Add(ProductViewModel model)
    {
        if (ModelState.IsValid)
        {
            model.Slug = Helper.GenerateSlug(model.Slug);
            if (_unitOfWork.ProductRepository.CheckSlugExist(model.Slug))
            {
                ModelState.AddModelError("Slug", "Slug is already existed");
                return View(model);
            }

            var product = new ProductModel()
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Content = model.Content,
                Description = model.Description,
                Slug = model.Slug,
            };
            if (model.ProductCategories != null)
            {
                foreach (var item in model.ProductCategories)
                {
                    var productCategory = _unitOfWork.ProductCategoryRepository.GetByID(item);
                    product.ProductCategories.Add(productCategory);
                }
            }

            if (model.ImageFile != null)
            {
                // var fileName = Helper.UploadFile(model.ImageFile, _hostingEnv.WebRootPath, Const.UPLOAD_IMAGE_DIR);
                var fileName = Helper.UploadFileCloud(model.ImageFile, _storage, _config);
                product.Image = fileName;
            }

            _unitOfWork.ProductRepository.Insert(product);
            _unitOfWork.Save();
            TempData["Message"] = "Create successfully";
            return RedirectToAction("Add");
        }
        model = InitProductViewModel(model, null);
        ViewData["menuActive"] = Const.ADMIN_MENU_PRODUCT;
        return View(model);
    }

    [HttpGet]
    public IActionResult Edit(Guid Id)
    {
        var product = _unitOfWork.ProductRepository.GetByID(Id);
        // Dumper.Dump(product.ProductCategories);
        if (product == null)
        {
            return NotFound();
        }
        var model = new ProductViewModel()
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Content = product.Content,
            Image = product.Image,
            Slug = product.Slug,
            UpdatedAt = product.UpdatedAt != null ? product.UpdatedAt!.Value : null,
            CreatedAt = product.CreatedAt != null ? product.CreatedAt!.Value : null,
        };

        model = InitProductViewModel(model, product);
        // Dumper.Dump(model);
        ViewData["menuActive"] = Const.ADMIN_MENU_PRODUCT;
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(ProductViewModel model, Guid Id)
    {
        var product = _unitOfWork.ProductRepository.GetByID(Id);
        if (product == null)
        {
            return NotFound();
        }
        if (ModelState.IsValid)
        {
            model.Slug = Helper.GenerateSlug(model.Slug);
            if (product.Slug != model.Slug && _unitOfWork.ProductRepository.CheckSlugExist(model.Slug))
            {
                ModelState.AddModelError("Slug", "Slug is already existed");
                model = InitProductViewModel(model, product);
                return View(model);
            }

            product.Name = model.Name;
            product.Description = model.Description;
            product.Content = model.Content;
            product.Slug = model.Slug;
            foreach (var item in product.ProductCategories)
            {
                product.ProductCategories.Remove(item);
            }
            if (model.ProductCategories != null)
            {
                var productCategories = _unitOfWork.ProductCategoryRepository.Get(x => model.ProductCategories.Contains(x.Id));
                foreach (var item in productCategories)
                {
                    product.ProductCategories.Add(item);
                }
            }
            if (model.ImageFile != null)
            {
                var fileName = Helper.UploadFileCloud(model.ImageFile, _storage, _config);
                product.Image = fileName;
            }

            _unitOfWork.ProductRepository.Update(product);
            _unitOfWork.Save();
            TempData["Message"] = "Update successfully";
            return RedirectToAction("Edit", new { Id });
        }
        model = InitProductViewModel(model, product);
        return View(model);
    }

    [HttpGet]
    public IActionResult Delete(Guid Id)
    {
        var product = _unitOfWork.ProductRepository.GetByID(Id);
        if (product == null)
        {
            return NotFound();
        }
        _unitOfWork.ProductRepository.Delete(product);
        _unitOfWork.Save();
        TempData["Message"] = "Delete successfully";
        return RedirectToAction("Index");
    }

    private ProductViewModel InitProductViewModel(ProductViewModel model, ProductModel? product)
    {
        var productCategorySelectListItems = _unitOfWork.ProductCategoryRepository.ToSelectListItems();
        model.ProductCategoryList = productCategorySelectListItems;

        if (product != null)
        {
            var productCategories = new List<Guid>() { };
            foreach (var item in product.ProductCategories)
            {
                productCategories.Add(item.Id);
            }
            model.ProductCategories = productCategories;
        }

        return model;
    }
}
