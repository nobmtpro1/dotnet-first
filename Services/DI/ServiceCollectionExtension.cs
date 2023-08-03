using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Blog.Services.Interfaces;
using Blog.Data;
using Blog.Services;

namespace Blog.Services.DI

{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddServiceCollection(this IServiceCollection services)
        {            
            return services;
        }
    }
}
