using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ItemPriceCharts.UI.WPF.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    internal sealed class BoolToVisibilityConverter : ValueConverter<bool, Visibility>
    {
        public bool Invert { get; set; }
        public bool Collapse { get; set; }

        public BoolToVisibilityConverter()
        {
            this.Collapse = true;
        }

        public override Visibility Convert(bool value, object parameter, CultureInfo culture)
        {
            var outcome = (this.Invert ? !value : value) ? Visibility.Visible : (this.Collapse ? Visibility.Collapsed : Visibility.Hidden);
            return outcome;
        }
    }
}
