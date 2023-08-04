

using System.IO;
using System.Threading.Tasks;
using Minio;

namespace Blog.Services.Interfaces;
public interface IStorage
{
    public Task List();
    public Task<string> Upload(string bucketName, Stream fileStream, string objectName, string contentType);
}