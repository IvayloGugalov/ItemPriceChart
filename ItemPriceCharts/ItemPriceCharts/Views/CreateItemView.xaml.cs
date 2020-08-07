using System;
using System.Windows;
using System.Windows.Input;

using ItemPriceCharts.UI.WPF.ViewModels;

namespace ItemPriceCharts.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for CreateItemView.xaml
    /// </summary>
    public partial class CreateItemView : Window
    {
        //private readonly CreateItemViewModel viewModel;

        public CreateItemView()
        {
            InitializeComponent();

            //this.DataContext = this.viewModel = viewModel ?? throw new ArgumentNullException();
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
