using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

using Services.Models;

namespace Services.Data
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        internal ModelsContext dbContext;
        internal DbSet<TEntity> dbSet;

        public Repository(ModelsContext dbContext)
        {
            this.dbContext = dbContext;
            this.dbSet = this.dbContext.Set<TEntity>();
            this.dbContext.Database.Migrate();
        }

        public virtual IEnumerable<TEntity> All(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = this.dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            var splitBy = new char[] { ',' };
            foreach (var includeProperty in includeProperties.Split(splitBy, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity GetById(object id)
        {
            return this.dbSet.Find(id);
        }

        public virtual void Add(TEntity entity)
        {
            this.dbSet.Add(entity);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            this.dbSet.Attach(entityToUpdate);
            this.dbContext.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public void Delete(object id)
        {
            TEntity entityToDelete = this.dbSet.Find(id);
            this.Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (this.dbContext.Entry(entityToDelete).State == EntityState.Deleted)
            {
                this.dbSet.Attach(entityToDelete);
            }

            this.dbSet.Remove(entityToDelete);
        }
    }
}
