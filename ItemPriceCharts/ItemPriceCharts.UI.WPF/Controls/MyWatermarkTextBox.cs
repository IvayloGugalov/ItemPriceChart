using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using Xceed.Wpf.Toolkit;

namespace ItemPriceCharts.UI.WPF.Controls
{
    public class MyWatermarkTextBox : Control
    {
        // Keep the xaml and c# naming equal
        private WatermarkTextBox watermarkTextBox;

        // Static, so the WPF resource can connect to the MyWatermarkTextBox.xaml
        static MyWatermarkTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MyWatermarkTextBox), new FrameworkPropertyMetadata(typeof(MyWatermarkTextBox)));
        }

        public override void OnApplyTemplate()
        {
            this.watermarkTextBox = Template.FindName(nameof(this.watermarkTextBox), this) as WatermarkTextBox ?? throw new ArgumentNullException(nameof(this.watermarkTextBox));
            this.watermarkTextBox.Watermark = this.WatermarkValue;

            // Enable binding to the view model.
            // Although setting a BindingMode = TwoWay, this will not have an affect when the control is instantiated, so the BindingMode is set in xaml.
            Binding textBoxBinding = new Binding
            {
                Path = new PropertyPath(nameof(this.TextBoxValue)),
                Source = this
            };

            this.watermarkTextBox.SetBinding(TextBox.TextProperty, textBoxBinding);

            base.OnApplyTemplate();
        }

        #region Set Text Value

        public string TextBoxValue
        {
            get { return (string)GetValue(TextBoxValueProperty); }
            set { SetValue(TextBoxValueProperty, value); }
        }

        public static readonly DependencyProperty TextBoxValueProperty =
            DependencyProperty.Register(
                name: nameof(TextBoxValue),
                propertyType: typeof(string),
                ownerType: typeof(MyWatermarkTextBox),
                typeMetadata: new PropertyMetadata(
                    defaultValue: string.Empty,
                    propertyChangedCallback: TextBoxValuePropertyChanged));

        public static readonly RoutedEvent TextBoxValueChangedEvent =
            EventManager.RegisterRoutedEvent(
                nameof(TextBoxValueChangedEvent),
                RoutingStrategy.Direct,
                typeof(RoutedPropertyChangedEventHandler<string>),
                typeof(MyWatermarkTextBox));

        private static void TextBoxValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is MyWatermarkTextBox myWatermarkTextBox)
            {
                myWatermarkTextBox.RaiseEvent(new RoutedPropertyChangedEventArgs<string>((string)e.OldValue, (string)e.NewValue, TextBoxValueChangedEvent));
            }
        }

        #endregion

        #region Set Watermark Value

        public string WatermarkValue
        {
            get { return (string)GetValue(WatermarkValueProperty); }
            set { SetValue(WatermarkValueProperty, value); }
        }

        public static readonly DependencyProperty WatermarkValueProperty =
            DependencyProperty.Register(
                name: nameof(WatermarkValue),
                propertyType: typeof(string),
                ownerType: typeof(MyWatermarkTextBox),
                typeMetadata: new PropertyMetadata(
                    defaultValue: string.Empty));

        #endregion
    }
}
