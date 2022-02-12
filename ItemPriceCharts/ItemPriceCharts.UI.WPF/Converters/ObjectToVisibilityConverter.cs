using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ItemPriceCharts.UI.WPF.Converters
{
    [ValueConversion(typeof(object), typeof(Visibility))]
    internal sealed class ObjectToVisibilityConverter : ValueConverter<object, Visibility>
    {
        public bool Invert { get; set; }
        public bool Collapse { get; set; }

        public ObjectToVisibilityConverter()
        {
            this.Collapse = true;
        }

        public override Visibility Convert(object value, object parameter, CultureInfo culture)
        {
            var result = (this.Invert ? value == null : value != null)
                ? Visibility.Visible
                : (this.Collapse ? Visibility.Collapsed : Visibility.Hidden);

            return result;
        }
    }
}
