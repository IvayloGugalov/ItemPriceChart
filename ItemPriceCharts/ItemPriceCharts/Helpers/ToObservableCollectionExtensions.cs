using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace UI.WPF.Helpers
{
    public static class ToObservableCollectionExtensions
    {
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
        {
            return new ObservableCollection<T>(enumerable);
        }
    }
}
