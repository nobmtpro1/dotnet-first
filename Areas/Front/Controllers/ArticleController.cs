using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Microsoft.Extensions.Logging;
using Blog.Services.Interfaces;
using Blog.Ultils;
using System.Threading.Tasks;
using System.Linq;
using System;
using System.Linq.Expressions;

namespace Blog.Areas.Front.Controllers;

[Area("Front")]
public class ArticleController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;


    public ArticleController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    [Route("/articles/{category?}")]
    public async Task<IActionResult> Index(int? page, string? category)
    {
        var articles = _unitOfWork.ArticleRepository.DbSet();
        if (!string.IsNullOrEmpty(category))
        {
            articles = articles.Where(s => s.ArticleCategories.Where(x => x.Slug == category).Any());
        }
        articles = articles.OrderByDescending(x => x.CreatedAt);
        var articlesPaginatedList = await PaginatedList<ArticleModel>.CreateAsync(articles, page ?? 1, 12);
        var articleCategories = _unitOfWork.ArticleCategoryRepository.Get(orderBy: q => q.OrderBy(d => d.CreatedAt));
        var articleCategory =  _unitOfWork.ArticleCategoryRepository.Get(filter: x => x.Slug == category).FirstOrDefault();
        ViewData["articleCategories"] = articleCategories;
        ViewData["articleCategory"] = articleCategory;
        return View(articlesPaginatedList);
    }

    [Route("/article/{slug}")]
    public IActionResult Detail(string slug)
    {
        var article = _unitOfWork.ArticleRepository.GetBySlug(slug);
        if (article == null)
        {
            return NotFound();
        }
        return View(article);
    }
}
