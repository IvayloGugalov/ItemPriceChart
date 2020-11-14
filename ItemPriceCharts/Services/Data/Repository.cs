﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

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
            this.dbContext.Database.CanConnectAsync();
        }

        public async Task<IEnumerable<TEntity>> All(
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
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public async Task<TEntity> FindAsync(int id) => await this.dbSet.FindAsync(id);

        public async Task<bool> IsExisting(int id) => await this.dbSet.FindAsync(id) != null;

        public void Add(TEntity entity)
        {
            this.dbSet.Add(entity);
        }

        public void Update(TEntity entityToUpdate)
        {
            this.dbSet.Update(entityToUpdate);
        }

        public void Delete(TEntity entityToDelete)
        {
            if (this.dbContext.Entry(entityToDelete).State == EntityState.Deleted)
            {
                this.dbSet.Attach(entityToDelete);
            }

            this.dbSet.Remove(entityToDelete);
        }
    }
}
