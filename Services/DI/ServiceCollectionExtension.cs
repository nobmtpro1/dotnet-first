using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Blog.Services.Interfaces;
using Blog.Services.Implementation;
using Blog.Data;

namespace Blog.Services.DI

{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServiceCollection(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IArticleCategoryService, ArticleCategoryService>();

            return services;
        }
    }
}
