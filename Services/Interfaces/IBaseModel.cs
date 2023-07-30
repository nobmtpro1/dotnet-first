using System;
using Blog.Data;

namespace Blog.Services.Interfaces
{
    public interface IBaseModel
    {
        public Guid Id { set; get; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
