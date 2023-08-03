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

    public ArticleController(ILogger<HomeController> logger, IArticleService articleService)
    {
        _logger = logger;
        _articleService = articleService;
    }

    [Route("/articles")]
    public async Task<IActionResult> Index(int? page)
    {
        var articles = await _articleService.Search(page ?? 1);
        ViewData["imagePath"] = Helper.BaseUrl(Request) + Const.UPLOAD_IMAGE_DIR;
        return View(articles);
    }
}
