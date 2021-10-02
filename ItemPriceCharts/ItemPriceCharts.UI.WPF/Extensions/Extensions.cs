using System;
using System.Collections.Generic;

namespace ItemPriceCharts.UI.WPF.Extensions
{
    public static class Extensions
    {
        public static bool IsAny<T>(this T obj, params T[] args)
        {
            return Array.IndexOf(args, obj) >= 0;
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var i in enumerable)
            {
                action(i);
            }

            return enumerable;
        }

        public static IDictionary<T, K> ForEach<T, K>(this IDictionary<T, K> dictionary, Action<T, K> action)
        {
            foreach (var (key, value) in dictionary)
            {
                action(key, value);
            }

            return dictionary;
        }
        
    }
}
