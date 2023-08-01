using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Const = Blog.Const;

namespace Blog.Data
{
    public static class ApplicationDbContextExtensions
    {
        public static void EnsureSeedDataForContext(this ApplicationDbContext context)
        {
            Seedup1(context);
        }

        private static void Seedup1(ApplicationDbContext context)
        {
            Console.WriteLine("_______________abc");

            IdentityRole? role;
            if (!context.Role.Any(x => x.Name == Const.ROLE_ADMIN))
            {
                role = new IdentityRole()
                {
                    Id = Guid.NewGuid().ToString("D"),
                    Name = Const.ROLE_ADMIN,
                    NormalizedName = Const.ROLE_ADMIN.ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString("D"),
                };
                context.Role.Add(role);
            }
            else
            {
                role = (from c in context.Role where c.Name == Const.ROLE_ADMIN select c).FirstOrDefault();
            }

            if (!context.User.Any(x => x.UserName == "admin@gmail.com"))
            {
                var admin = new UserModel()
                {
                    Id = Guid.NewGuid().ToString("D"),
                    FullName = "Admin",
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    NormalizedEmail = "admin@gmail.com".ToUpper(),
                    EmailConfirmed = true,
                    ConcurrencyStamp = Guid.NewGuid().ToString("D"),
                    SecurityStamp = Guid.NewGuid().ToString("D"),
                    NormalizedUserName = "admin@gmail.com".ToUpper(),
                };
                string password = "123456";
                var passwordHash = new PasswordHasher<UserModel>();
                admin.PasswordHash = passwordHash.HashPassword(admin, password);

                context.User.Add(admin);
                if (role != null)
                {
                    if (!context.UserRole.Any(x => x.UserId == admin.Id && x.UserId == role.Id))
                    {
                        var userRole = new IdentityUserRole<string>()
                        {
                            UserId = admin.Id,
                            RoleId = role.Id,
                        };
                        context.UserRole.Add(userRole);
                    }
                }

            }


            if (!context.ArticleCategory.Any(x => x.Name == "ABC"))
            {
                var articleCategory = new ArticleCategoryModel()
                {
                    Id = Guid.NewGuid(),
                    Name = "ABC",
                    Slug = "ABC".ToUpper(),
                };
                context.ArticleCategory.Add(articleCategory);
            }

            context.SaveChanges();
        }
    }
}
