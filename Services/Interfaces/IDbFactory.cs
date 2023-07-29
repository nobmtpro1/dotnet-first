using System;
using Blog.Data;

namespace Blog.Services.Interfaces
{
    public interface IDbFactory : IDisposable
    {
        ApplicationDbContext Init();
    }
}
