using ItemPriceCharts.Services.Data;
using ItemPriceCharts.Services.Models;
using Moq;
using Moq.Language.Flow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ItemPriceCharts.ServicesTest.MockHelper
{
    public static class MockMethods
    {
        public static void GetAll(Mock<IRepository<IEntity>> mock)
        {
            
            //setup.GetAll()

            mock.Setup(_ => _.All(
                It.IsAny<Expression<Func<IEntity, bool>>>(),
                It.IsAny<Func<IQueryable<IEntity>, IOrderedQueryable<IEntity>>>(),
                It.IsAny<string>()));
        }
    }
}
