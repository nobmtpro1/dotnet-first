using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Microsoft.Extensions.Logging;
using Blog.Services.Interfaces;
using Blog.Ultils;
using System.Threading.Tasks;

namespace Blog.Areas.Front.Controllers;

[Area("Front")]
public class ArticleController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private IArticleService _articleService;
    private IArticleCategoryService _articleCategoryService;

    public ArticleController(ILogger<HomeController> logger, IArticleService articleService, IArticleCategoryService articleCategoryService)
    {
        _logger = logger;
        _articleService = articleService;
        _articleCategoryService = articleCategoryService;
    }

    [Route("/articles")]
    public async Task<IActionResult> Index(int? page)
    {
        var articles = await _articleService.Search(page ?? 1, null!);
        var articleCategories = _articleCategoryService.GetAll();
        ViewData["imagePath"] = Helper.BaseUrl(Request) + Const.UPLOAD_IMAGE_DIR;
        ViewData["articleCategories"] = articleCategories;
        return View(articles);
    }

    [Route("/article/category/{slug}")]
    public async Task<IActionResult> Category(string slug, int? page)
    {
        var articles = await _articleService.Search(page ?? 1, slug);
        var articleCategories = _articleCategoryService.GetAll();
        ViewData["imagePath"] = Helper.BaseUrl(Request) + Const.UPLOAD_IMAGE_DIR;
        ViewData["articleCategories"] = articleCategories;
        return View("Index", articles);
    }

    [Route("/article/{slug}")]
    public IActionResult Detail(string slug)
    {
        var article = _articleService.GetBySlug(slug);
        if (article == null)
        {
            return NotFound();
        }
        ViewData["imagePath"] = Helper.BaseUrl(Request) + Const.UPLOAD_IMAGE_DIR;
        return View(article);
    }
}
