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
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
    }
}
