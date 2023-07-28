using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Blog.Models
{

    public class UserModel : IdentityUser
    {
        [MaxLength(100)]
        public string? FullName { set; get; }

        [MaxLength(255)]
        public string? Address { set; get; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}