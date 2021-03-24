using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Data
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : EntityModel
    {
        public async Task<IEnumerable<TEntity>> GetAll(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            using (ModelsContext context = new ModelsContext())
            {
                IQueryable<TEntity> query = context.Set<TEntity>();

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
        }

        public async Task<TEntity> FindAsync(int id)
        {
            using (ModelsContext dbContext = new ModelsContext())
            {
                return await dbContext.Set<TEntity>().FindAsync(id);
            }
        }

        public async Task<bool> IsEntityExistingByAttribute(Expression<Func<TEntity, bool>> filter)
        {
            using (ModelsContext dbContext = new ModelsContext())
            {
                return await dbContext.Set<TEntity>().FirstOrDefaultAsync(filter) != null;
            }
        }

        public async Task<bool> IsExisting(int id)
        {
            using (ModelsContext dbContext = new ModelsContext())
            {
                return await dbContext.Set<TEntity>().FindAsync(id) != null;
            }
        }

        public async Task<TEntity> Add(TEntity entity)
        {
            using (ModelsContext dbContext = new ModelsContext())
            {
                dbContext.Set<TEntity>().Attach(entity);
                EntityEntry<TEntity> createdEntity = dbContext.Add(entity);
                await dbContext.SaveChangesAsync();

                return createdEntity.Entity;
            }
        }

        public async Task<TEntity> Update(TEntity entityToUpdate)
        {
            using (ModelsContext dbContext = new ModelsContext())
            {
                dbContext.Set<TEntity>().Update(entityToUpdate);
                await dbContext.SaveChangesAsync();

                return entityToUpdate;
            }
        }

        public async Task<bool> Delete(TEntity entityToDelete)
        {
            using (ModelsContext dbContext = new ModelsContext())
            {
                if (dbContext.Entry(entityToDelete).State == EntityState.Deleted)
                {
                    dbContext.Set<TEntity>().Attach(entityToDelete);
                }

                dbContext.Set<TEntity>().Remove(entityToDelete);
                await dbContext.SaveChangesAsync();

                return true;
            }
        }
    }
}
