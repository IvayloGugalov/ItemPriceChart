using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Moq;

using Microsoft.EntityFrameworkCore;

namespace ItemPriceCharts.ServicesTest.MockHelper
{
    public static class DbSetMock
    {
        public static Mock<DbSet<T>> GetQueryableMockDbSet<T>(List<T> source) where T : class
        {
            var queryable = source.AsQueryable();
            var dbSet = new Mock<DbSet<T>>();

            dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
            dbSet.Setup(m => m.AddAsync(It.IsAny<T>(), It.IsAny<CancellationToken>())).Callback<T>((s) => source.Add(s));

            return dbSet;
        }
    }
}
