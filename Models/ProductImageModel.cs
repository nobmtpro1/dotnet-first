using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Blog.Services.Interfaces;

namespace Blog.Models
{
    [Table("ProductImage")]
    public class ProductImageModel : BaseModel
    {
        public string ProductId { set; get; } = default!;
        public string Src { set; get; } = default!;
    }
}