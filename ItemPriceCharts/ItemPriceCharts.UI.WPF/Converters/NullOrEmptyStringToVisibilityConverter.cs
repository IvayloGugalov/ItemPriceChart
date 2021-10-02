using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ItemPriceCharts.UI.WPF.Converters
{
    [ValueConversion(typeof(string), typeof(Visibility))]

    internal sealed class NullOrEmptyStringToVisibilityConverter : ValueConverter<string, Visibility>
    {
        public Visibility TrueValue { get; set; }
        public Visibility FalseValue { get; set; }

        public NullOrEmptyStringToVisibilityConverter()
        {
            this.TrueValue = Visibility.Visible;
            this.FalseValue = Visibility.Collapsed;
        }

        public override Visibility Convert(string value, object parameter, CultureInfo culture)
        {
            var result = string.IsNullOrEmpty(value) ? Visibility.Collapsed : Visibility.Visible;
            return result;
        }
    }
}
