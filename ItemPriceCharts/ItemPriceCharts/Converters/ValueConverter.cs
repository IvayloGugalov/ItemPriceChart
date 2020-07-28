using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace UI.WPF.Converters
{
    [MarkupExtensionReturnType(typeof(IValueConverter))]
    internal abstract class ValueConverter<TSource, TTarget> : MarkupExtension, IValueConverter
    {
        public virtual TTarget Convert(TSource value, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"{this.GetType()} , {typeof(TSource)}, {typeof(TTarget)}");
        }

        public virtual TSource ConvertBack(TTarget value, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"{this.GetType()}, {typeof(TTarget)}, {typeof(TSource)}");
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.Convert((TSource)value, parameter, culture);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.ConvertBack((TTarget)value, parameter, culture);
        }
    }
}
