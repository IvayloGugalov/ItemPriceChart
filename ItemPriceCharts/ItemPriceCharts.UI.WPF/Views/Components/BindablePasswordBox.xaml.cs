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

        public BindablePasswordBox()
        {
            InitializeComponent();
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
    }
}
