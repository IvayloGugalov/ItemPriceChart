using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ItemPriceCharts.UI.WPF.Views.Components
{
    /// <summary>
    /// Interaction logic for BindablePasswordBox.xaml
    /// </summary>
    public partial class BindablePasswordBox : UserControl
    {
        private bool isPasswordChanging;

        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(
                name: nameof(Password),
                propertyType: typeof(string),
                ownerType: typeof(BindablePasswordBox),
                typeMetadata: new FrameworkPropertyMetadata(
                    defaultValue: string.Empty,
                    flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    propertyChangedCallback: PasswordProperyChanged,
                    coerceValueCallback: null,
                    isAnimationProhibited: false,
                    defaultUpdateSourceTrigger: UpdateSourceTrigger.PropertyChanged));


        public string Watermark
        {
            get => (string)GetValue(WatermarkProperty);
            set => SetValue(WatermarkProperty, value);
        }

        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register(
                name: nameof(Watermark),
                propertyType: typeof(string),
                ownerType: typeof(BindablePasswordBox),
                typeMetadata: new PropertyMetadata(
                    defaultValue: "Password",
                    propertyChangedCallback: WatermarkPropertyChanged));

        public BindablePasswordBox()
        {
            InitializeComponent();

            this.passwordBox.Watermark = this.Watermark;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            this.isPasswordChanging = true;

            this.Password = this.passwordBox.Password;

            this.isPasswordChanging = false;
        }

        private void UpdatePassword()
        {
            if (!this.isPasswordChanging)
            {
                this.passwordBox.Password = this.Password;
            }
        }

        private static void PasswordProperyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindablePasswordBox bindablePasswordBox)
            {
                bindablePasswordBox.UpdatePassword();
            }
        }

        private static void WatermarkPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindablePasswordBox bindablePasswordBox)
            {
                bindablePasswordBox.UpdateWatermark();
            }
        }

        private void UpdateWatermark()
        {
            this.passwordBox.Watermark = this.Watermark;
        }
    }
}
