using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ItemPriceCharts.UI.WPF.Extensions
{
    public static class ToObservableCollectionExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
        {
            return new(enumerable);
        }
    }
}
