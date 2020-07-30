using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ItemPriceCharts.Services.Data
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity GetById(object id);
        IEnumerable<TEntity> All(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>,
            IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(object entity);
    }
}
