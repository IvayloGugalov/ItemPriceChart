using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using UI.WPF.ViewModels;

namespace UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for CreateShopView.xaml
    /// </summary>
    public partial class CreateShopView : Window
    {
        private CreateShopViewModel viewModel;

        public CreateShopView(CreateShopViewModel viewModel)
        {
            InitializeComponent();

            this.DataContext = this.viewModel = viewModel ?? throw new ArgumentNullException();
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
