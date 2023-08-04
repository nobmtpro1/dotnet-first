using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Blog.Models;
using System.Linq.Expressions;
using Blog.Data;
using Microsoft.EntityFrameworkCore;

namespace Blog.Services.Interfaces
{
    public interface IBaseRepository<TEntity> where TEntity : class
    {
        public IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null!,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null!,
            string includeProperties = "");

        public TEntity GetByID(object id);

        public TEntity Insert(TEntity entity);

        public TEntity Delete(object id);

        public TEntity Delete(TEntity entityToDelete);

        public TEntity Update(TEntity entityToUpdate);
    }
}