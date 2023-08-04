using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Microsoft.Extensions.Logging;
using Blog.Services.Interfaces;
using System.Threading.Tasks;
using Minio;
using System;

namespace Blog.Areas.Front.Controllers;

[Area("Front")]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IStorage _storage;

    public HomeController(ILogger<HomeController> logger, IStorage storage)
    {
        _logger = logger;
        _storage = storage;
    }

    public async Task<IActionResult> Index()
    {
        await _storage.List();
        // await _storage.Upload();
        // var minio = new MinioClient().WithEndpoint("localhost:9000").WithCredentials("minio", "miniostorage").Build();
        // var getListBucketsTask = await minio.ListBucketsAsync();
        // foreach (var bucket in getListBucketsTask.Buckets)
        // {
        //     Console.WriteLine(bucket.Name + " " + bucket.CreationDateDateTime);
        // }
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
}
