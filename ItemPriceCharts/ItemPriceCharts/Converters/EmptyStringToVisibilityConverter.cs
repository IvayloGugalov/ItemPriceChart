using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ItemPriceCharts.UI.WPF.Converters
{
    [ValueConversion(typeof(string), typeof(Visibility))]

    internal sealed class EmptyStringToVisibilityConverter : IValueConverter
    {
        public Visibility TrueValue { get; set; }
        public Visibility FalseValue { get; set; }

        public EmptyStringToVisibilityConverter()
        {
            this.TrueValue = Visibility.Visible;
            this.FalseValue = Visibility.Collapsed;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = string.IsNullOrEmpty(value as string) ? Visibility.Collapsed : Visibility.Visible;
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (object.Equals(value, this.TrueValue))
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
