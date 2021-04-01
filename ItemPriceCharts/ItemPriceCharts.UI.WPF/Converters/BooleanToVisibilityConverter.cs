using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ItemPriceCharts.UI.WPF.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public sealed class BooleanToVisibilityConverter : IValueConverter
    {
        public Visibility TrueValue { get; set; }
        public Visibility FalseValue { get; set; }

        public BooleanToVisibilityConverter()
        {
            this.TrueValue = Visibility.Visible;
            this.FalseValue = Visibility.Collapsed;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool b)
            {
                return b ? this.TrueValue : this.FalseValue;
            }

            return null;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if(object.Equals(value, this.TrueValue))
            {
                return true;
            }

            if (object.Equals(value, this.FalseValue))
            {
                return false;
            }

            return null;
        }
    }
}
