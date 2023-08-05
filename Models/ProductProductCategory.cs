using System;
using System.ComponentModel.DataAnnotations;
using Blog.Services.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    [Table("ProductProductCategory")]
    public class ProductProductCategoryModel : BaseModel
    {
        public Guid ProductId { get; set; }
        public Guid ProductCategoryId { get; set; }
        public virtual ProductModel Product { get; set; } = default!;
        public virtual ProductCategoryModel ProductCategory { get; set; } = default!;
    }
}