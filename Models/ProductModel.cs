using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Blog.Services.Interfaces;

namespace Blog.Models
{
    [Table("Product")]
    public class ProductModel : BaseModel
    {
        public ProductModel()
        {
            ProductCategories = new HashSet<ProductCategoryModel>();
            ProductProductCategory = new HashSet<ProductProductCategoryModel>();
            ProductImages = new HashSet<ProductImageModel>();
        }
        public string Name { set; get; } = default!;

        public string? Description { set; get; }
        public string? Content { set; get; }

        public string? Image { set; get; }

        public string Slug { set; get; } = default!;

        public virtual ICollection<ProductCategoryModel> ProductCategories { get; set; }
        public virtual ICollection<ProductProductCategoryModel> ProductProductCategory { get; set; }
        public virtual ICollection<ProductImageModel> ProductImages { get; set; }
    }
}