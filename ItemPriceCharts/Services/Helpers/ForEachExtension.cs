using System;
using System.Collections.Generic;

namespace ItemPriceCharts.Services.Helpers
{
    public static class ForEachExtension
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var i in enumerable)
            {
                action(i);
            }

            return enumerable;
        }
    }
}
