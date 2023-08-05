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
using System.Data;

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
            .HasForeignKey(g => g.ParentId);

            modelBuilder.Entity<ArticleModel>()
            .HasMany(s => s.ArticleCategories)
            .WithMany(c => c.Articles);

            modelBuilder.Entity<ArticleModel>()
            .HasIndex(u => u.Slug)
            .IsUnique();

            modelBuilder.Entity<ProductModel>()
            .HasMany(e => e.ProductCategories)
            .WithMany(e => e.Products)
            .UsingEntity(
                "ProductProductCategoryModel",
                l => l.HasOne(typeof(ProductCategoryModel)).WithMany().HasForeignKey("ProductCategoryId").HasPrincipalKey(nameof(ProductCategoryModel.Id)),
                r => r.HasOne(typeof(ProductModel)).WithMany().HasForeignKey("ProductId").HasPrincipalKey(nameof(ProductModel.Id)),
                j => j.HasKey("ProductId", "ProductCategoryId")
            );

            modelBuilder.Entity<ProductCategoryModel>()
            .HasOne(g => g.Parent)
            .WithMany(l => l.Children)
            .HasForeignKey(g => g.ParentId);

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