using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UI.WPF.ViewModels;

namespace UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for CreateItemView.xaml
    /// </summary>
    public partial class CreateItemView : Window
    {
        private readonly CreateItemViewModel viewModel;

        public CreateItemView(CreateItemViewModel viewModel)
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
