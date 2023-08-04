using System;
using System.IO;
using System.Threading.Tasks;
using Blog.Services.Interfaces;
using Blog.Ultils;
using Minio;
using Minio.Exceptions;

namespace Blog.Services.Implementation;
public class MinioService : IStorage
{
    private readonly IMinioClient _client;
    public MinioService(IMinioClient client)
    {
        _client = client;

    }
    public async Task List()
    {
        var listBucketResponse = await _client.ListBucketsAsync();
        foreach (var bucket in listBucketResponse.Buckets)
        {
            Console.Out.WriteLine("bucket '" + bucket.Name + "' created at " + bucket.CreationDate);
        }
    }

    private async static Task Init(IMinioClient _client, string bucketName)
    {
        try
        {
            var beArgs = new BucketExistsArgs()
                .WithBucket(bucketName);
            bool found = await _client.BucketExistsAsync(beArgs).ConfigureAwait(false);
            if (!found)
            {
                var mbArgs = new MakeBucketArgs()
                    .WithBucket(bucketName);
                await _client.MakeBucketAsync(mbArgs).ConfigureAwait(false);
            }
        }
        catch (MinioException e)
        {
            Console.WriteLine("File Upload Error: {0}", e.Message);
        }
    }

    public async Task<string> Upload(string bucketName, Stream fileStream, string objectName, string contentType)
    {
        Init(_client, bucketName).Wait();
        var putObjectArgs = new PutObjectArgs()
            .WithBucket(bucketName)
            .WithObject(objectName)
            .WithStreamData(fileStream)
            .WithObjectSize(fileStream.Length)
            .WithContentType(contentType);
        var res = await _client.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
        Helper.Dump(res);
        Console.WriteLine("Successfully uploaded " + objectName);
        return res.ObjectName;
    }
}