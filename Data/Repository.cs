using Blog.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Blog.Data
{
    public abstract class Repository<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
    {
        protected ApplicationDbContext DataContext;
        protected DbSet<TEntity> DbSet;

        protected Repository(ApplicationDbContext context)
        {
            DataContext = context;
            DbSet = DataContext.Set<TEntity>();
        }

        public IQueryable<TEntity> GetAll()
        {
            return DataContext.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetByIds(IEnumerable<TKey> ids, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = DataContext.Set<TEntity>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.Where(i => ids.Contains(i.Id));
        }

        public TEntity GetById(TKey id, params Expression<Func<TEntity, object>>[] includes)
        {
            //return GetByIds(new[] { id }, includes).FirstOrDefault();
            return DataContext.Set<TEntity>().Find(id)!;
        }

        public IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = DataContext.Set<TEntity>();
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.Where(predicate).AsEnumerable();
        }

        public void Insert(TEntity entity)
        {
            DataContext.Set<TEntity>().Add(entity);
        }

        public void Insert(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                DataContext.Set<TEntity>().Add(entity);
            }
        }

        public void Update(TEntity entity)
        {
            DataContext.Entry(entity).State = EntityState.Modified;
        }

        public void Update(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                DataContext.Entry(entity).State = EntityState.Modified;
            }
        }

        public void Update(TEntity entity, params Expression<Func<TEntity, object>>[] properties)
        {
            DataContext.Set<TEntity>().Attach(entity);
            EntityEntry<TEntity> entry = DataContext.Entry(entity);
            foreach (var selector in properties)
            {
                entry.Property(selector).IsModified = true;
            }
        }

        public virtual void Delete(TKey id)
        {
            var entity = DbSet.Find(id);
            DbSet.Remove(entity!);
        }

        public virtual void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public void DeleteMulti(IEnumerable<TKey> ids)
        {
            foreach (var id in ids)
            {
                var entity = DbSet.Find(id);
                DbSet.Remove(entity!);
            }
        }

        public virtual void DeleteMulti(IEnumerable<TEntity> entities)
        {
            foreach (TEntity obj in entities)
                DbSet.Remove(obj);
        }

    }
}