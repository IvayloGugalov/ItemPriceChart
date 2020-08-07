using System;
using System.Linq.Expressions;

namespace ItemPriceCharts.UI.WPF.Helpers
{
    public static class ExpressionHelper
    {
        public static string PropertyName<T>(Expression<Func<T>> expression)
        {
            var lambda = expression as LambdaExpression;
            var memberExpression = (MemberExpression)lambda.Body;

            return memberExpression.Member.Name;
        }
    }
}
