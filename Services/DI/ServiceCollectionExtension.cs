using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Blog.Services.Interfaces;
using Blog.Data;
using Blog.Services;
using Blog.Services.Repository;

namespace Blog.Services.DI

{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServiceCollection(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IArticleCategoryRepository, ArticleCategoryRepository>();
   
            return services;
        }
    }
}
