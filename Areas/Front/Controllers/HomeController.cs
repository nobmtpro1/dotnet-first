using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Microsoft.Extensions.Logging;
using Blog.Services.Interfaces;
using System.Threading.Tasks;
using Minio;
using System;
using System.Net.Http;
using Microsoft.Net.Http.Headers;
using System.Text.Json;
using System.Collections.Generic;
using Blog.Ultils;
using Newtonsoft.Json;

namespace Blog.Areas.Front.Controllers;

[Area("Front")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(ILogger<HomeController> logger, IHttpClientFactory httpClientFactory, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public async Task<object> SeedArticle()
    {
        var articleCategories = new List<ArticleCategoryModel>(){
            new ArticleCategoryModel(){
                Id = Guid.NewGuid(),
                Name = "Marketing Management",
                Slug = Helper.GenerateSlug("Marketing Management"),
            },
            new ArticleCategoryModel(){
                Id = Guid.NewGuid(),
                Name = "Creative Communication",
                Slug = Helper.GenerateSlug("Creative Communication"),
            },
            new ArticleCategoryModel(){
                Id = Guid.NewGuid(),
                Name = "Digital Marketing",
                Slug = Helper.GenerateSlug("Digital Marketing"),
            },
            new ArticleCategoryModel(){
                Id = Guid.NewGuid(),
                Name = "Framework",
                Slug = Helper.GenerateSlug("Framework"),
            },
            new ArticleCategoryModel(){
                Id = Guid.NewGuid(),
                Name = "News from Cannes",
                Slug = Helper.GenerateSlug("News from Cannes"),
            },
        };

        foreach (var item in articleCategories)
        {
            if (!_unitOfWork.ArticleCategoryRepository.CheckSlugExist(item.Slug))
            {
                _unitOfWork.ArticleCategoryRepository.Insert(item);
                _unitOfWork.Save();
            }
        }

        var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get, "https://610e5b4548beae001747bad5.mockapi.io/products"
        );

        var httpClient = _httpClientFactory.CreateClient();
        var response = await httpClient.SendAsync(httpRequestMessage);
        object? data = null;
        if (response.IsSuccessStatusCode)
        {
            data = JsonConvert.DeserializeObject<List<object>>(await response.Content.ReadAsStringAsync());
            // Helper.Dump(data);
            List<ArticleCategoryModel> allSrticleCategories = (List<ArticleCategoryModel>)_unitOfWork.ArticleCategoryRepository.Get();
            foreach (var item in (dynamic)data!)
            {
                var slug = Helper.GenerateSlug((string)item.title);
                Helper.Dump(slug);
                Helper.Dump(_unitOfWork.ArticleRepository.CheckSlugExist(slug));
                if (_unitOfWork.ArticleRepository.CheckSlugExist(slug))
                {
                    Helper.Dump(slug);
                    continue;
                }
                else
                {
                    var article = new ArticleModel()
                    {
                        Id = Guid.NewGuid(),
                        Title = item.title,
                        Content = item.content,
                        Image = item.image,
                        Slug = slug,

                    };
                    Random rnd = new Random();
                    int r = rnd.Next(allSrticleCategories.Count);
                    article.ArticleCategories.Add(allSrticleCategories[r]);
                    _unitOfWork.ArticleRepository.Insert(article);
                    _unitOfWork.Save();
                    article.CreatedAt = item.created_at;
                    _unitOfWork.ArticleRepository.Update(article);
                    _unitOfWork.Save();
                }
            }
        }
        return Ok(data);
    }
}
