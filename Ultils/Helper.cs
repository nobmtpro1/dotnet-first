using System;
using System.IO;
using System.Text.RegularExpressions;
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
        var uploadPath = Path.Combine(WebRootPath, uploadDirectory);

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

    public static string GenerateSlug(this string phrase)
    {
        string str = phrase.RemoveAccent().ToLower();
        // invalid chars           
        str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
        // convert multiple spaces into one space   
        str = Regex.Replace(str, @"\s+", " ").Trim();
        // cut and trim 
        str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
        str = Regex.Replace(str, @"\s", "-"); // hyphens   
        return str;
    }

    public static string RemoveAccent(this string txt)
    {
        byte[] bytes = System.Text.Encoding.GetEncoding("Cyrillic").GetBytes(txt);
        return System.Text.Encoding.ASCII.GetString(bytes);
    }
}