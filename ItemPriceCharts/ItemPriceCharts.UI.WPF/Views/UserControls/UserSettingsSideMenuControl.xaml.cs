using System.Windows;
using System.Windows.Controls;

namespace ItemPriceCharts.UI.WPF.Views.UserControls
{
    /// <summary>
    /// Interaction logic for UserSettingsSideMenuControl.xaml
    /// </summary>
    public partial class UserSettingsSideMenuControl : UserControl
    {
        public UserSettingsSideMenuControl()
        {
            InitializeComponent();

            buttonsContainer.Visibility = Visibility.Collapsed;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.buttonsContainer.Visibility = this.buttonsContainer.Visibility != Visibility.Visible
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
    }
}
