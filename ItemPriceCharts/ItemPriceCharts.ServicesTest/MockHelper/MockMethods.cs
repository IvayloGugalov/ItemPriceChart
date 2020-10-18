using ItemPriceCharts.Services.Data;
using Moq;
using Moq.Language.Flow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ItemPriceCharts.ServicesTest.MockHelper
{
    public static class MockMethods<T>
        where T : class
    {
        public static IReturnsResult<IRepository<T>> GetAll(Mock<IRepository<T>> mock, List<T> items)
        {
            return mock.Setup(_ => _.All(
                It.IsAny<Expression<Func<T, bool>>>(),
                It.IsAny<Func<IQueryable<T>, IOrderedQueryable<T>>>(),
                It.IsAny<string>()))
                .ReturnsAsync(items);
        }
    }
}
