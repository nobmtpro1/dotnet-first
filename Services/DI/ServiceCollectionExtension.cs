using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Blog.Services.Interfaces;
using Blog.Data;
using Blog.Services;
using Blog.Services.Repository;
using Blog.Services.Implementation;
using Minio;
using Microsoft.Extensions.Configuration;

namespace Blog.Services.DI

{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServiceCollection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IStorage, MinioService>();
            services.AddSingleton<IMinioClient>(sp =>
            {
                var section = configuration.GetSection("Minio");
                var endpoint = section["Endpoint"];
                var key = section["AccessKey"];
                var secret = section["SecretKey"];
                return new MinioClient().WithEndpoint(endpoint).WithCredentials(key, secret).Build();
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IArticleCategoryRepository, ArticleCategoryRepository>();

            return services;
        }
    }
}
