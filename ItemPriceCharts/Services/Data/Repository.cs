using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Data
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
            if (!this.dbContext.Database.CanConnect())
            {
                throw new Exception("Can't connect to Database");
            }
        }

        public virtual async Task<IEnumerable<TEntity>> All(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
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
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public virtual async Task<TEntity> GetById(int id) => await this.dbSet.FindAsync(id);

        public virtual async Task Add(TEntity entity)
        {
            await this.dbSet.AddAsync(entity);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            this.dbSet.Attach(entityToUpdate);
            this.dbContext.Entry(entityToUpdate).State = EntityState.Modified;
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
