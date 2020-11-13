using System;
using System.Windows;
using System.Windows.Input;

namespace ItemPriceCharts.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for CreateShopView.xaml
    /// </summary>
    public partial class CreateShopView : Window
    {
        public CreateShopView()
        {
            InitializeComponent();

            this.createShop.Click += (s, e) => this.CloseButton_Click(s, e);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
