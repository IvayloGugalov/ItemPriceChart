using System.Windows;
using System.Windows.Controls;

namespace ItemPriceCharts.UI.WPF.Views.UserControls
{
    /// <summary>
    /// Interaction logic for SideMenuView.xaml
    /// </summary>
    public partial class SideMenuView : UserControl
    {
        public SideMenuView()
        {
            InitializeComponent();

            this.listOfShops.Visibility = Visibility.Collapsed;
            this.ArrowUp.Visibility = Visibility.Collapsed;

            this.buttonsContainer.Visibility = Visibility.Collapsed;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            this.buttonsContainer.Visibility = this.buttonsContainer.Visibility != Visibility.Visible
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (this.listOfShops.Visibility != Visibility.Visible)
            {
                this.listOfShops.Visibility = Visibility.Visible;
                this.ArrowDown.Visibility = Visibility.Collapsed;
                this.ArrowUp.Visibility = Visibility.Visible;
            }
            else
            {
                this.listOfShops.Visibility = Visibility.Collapsed;
                this.ArrowUp.Visibility = Visibility.Collapsed;
                this.ArrowDown.Visibility = Visibility.Visible;
            }
        }
    }
}
