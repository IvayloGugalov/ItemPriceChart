using System.Globalization;
using System.Windows.Data;

namespace ItemPriceCharts.UI.WPF.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    internal sealed class InverseBoolConverter : ValueConverter<bool, bool>
    {
        public override bool Convert(bool value, object parameter, CultureInfo culture) => !value;
    }
}
