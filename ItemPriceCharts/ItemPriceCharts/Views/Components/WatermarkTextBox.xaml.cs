using System.Windows;
using System.Windows.Controls;

namespace ItemPriceCharts.UI.WPF.Views.Components
{
    /// <summary>
    /// Interaction logic for WatermarkTextBox.xaml
    /// </summary>
    public partial class WatermarkTextBox : UserControl
    {

        public CornerRadius BorderCornerRadius
        {
            get { return (CornerRadius)GetValue(BorderCornerRadiusProperty); }
            set { SetValue(BorderCornerRadiusProperty, value); }
        }

        public static DependencyProperty BorderCornerRadiusProperty =
            DependencyProperty.Register(nameof(BorderCornerRadius), typeof(CornerRadius), typeof(BindablePasswordBox));

        public WatermarkTextBox()
        {
            InitializeComponent();
        }
    }
}
