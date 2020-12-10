using System.Windows;

namespace ItemPriceCharts.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.listOfShops.Visibility = Visibility.Collapsed;
            this.ArrowUp.Visibility = Visibility.Collapsed;
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
