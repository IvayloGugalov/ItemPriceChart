using System.Collections.Generic;
using System.Linq;

using Moq;

using Microsoft.EntityFrameworkCore;

namespace ItemPriceCharts.ServicesTest.MockHelper
{
    public static class DbSetMock
    {
        public static DbSet<T> GetQueryableMockDbSet<T>(IEnumerable<T> source) where T : class
        {
            var queryable = source.AsQueryable();
            var dbSet = new Mock<DbSet<T>>();

            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            //dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));

            return dbSet.Object;
        }
    }
}
