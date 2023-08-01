using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Blog.Ultils;
public static class Helper
{
    public static void Dump(this object obj)
    {
        Console.WriteLine(JsonConvert.SerializeObject(obj, Formatting.Indented,
        new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        })); // your logger
    }

    public static string UploadFile(IFormFile file, string WebRootPath, string uploadDirectory)
    {
        var uploadDir = "uploads/" + uploadDirectory;
        var uploadPath = Path.Combine(WebRootPath, uploadDir);

        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var filePath = Path.Combine(uploadPath, fileName);

        using (var strem = File.Create(filePath))
        {
            file.CopyTo(strem);
        }
        return fileName;
    }

    public static string BaseUrl(HttpRequest Request)
    {
        return $"{Request.Scheme}://{Request.Host}{Request.PathBase}/";
    }
}