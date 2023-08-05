using System;
using System.ComponentModel.DataAnnotations;
using Blog.Services.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    [Table("ProductCategory")]
    public class ProductCategoryModel : BaseModel
    {

        public ProductCategoryModel()
        {
            Products = new HashSet<ProductModel>();
            ProductProductCategory = new HashSet<ProductProductCategoryModel>();
        }
        public string Name { set; get; } = default!;

        public string Slug { set; get; } = default!;

        public Guid? ParentId { set; get; }

        public virtual ProductCategoryModel? Parent { get; set; } = null!;
        public virtual ICollection<ProductCategoryModel> Children { get; set; } = new List<ProductCategoryModel>();

        public virtual ICollection<ProductModel> Products { get; set; }
        public virtual ICollection<ProductProductCategoryModel> ProductProductCategory { get; set; }
    }
}