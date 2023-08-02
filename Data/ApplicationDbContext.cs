using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Blog.Services.Interfaces;

namespace Blog.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArticleCategoryModel>()
            .HasOne(g => g.Parent)
            .WithMany(l => l.Children)
            .HasForeignKey(g => g.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ArticleModel>()
            .HasMany<ArticleCategoryModel>(s => s.ArticleCategories)
            .WithMany(c => c.Articles);

            modelBuilder.Entity<ArticleModel>()
            .HasIndex(u => u.Slug)
            .IsUnique();

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries().Where(e => e.Entity is BaseModel && (
             e.State == EntityState.Added
             || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                ((BaseModel)entityEntry.Entity).UpdatedAt = DateTime.Now;

                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseModel)entityEntry.Entity).CreatedAt = DateTime.Now;
                }
            }
            return base.SaveChanges();
        }

        public DbSet<UserModel> User { get; set; } = default!;
        public DbSet<IdentityUserRole<string>> UserRole { get; set; } = default!;
        public DbSet<IdentityRole> Role { get; set; } = default!;
        public DbSet<ArticleModel> Article { get; set; } = default!;
        public DbSet<ArticleCategoryModel> ArticleCategory { get; set; } = default!;
    }
}