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
    /// Interaction logic for DeleteShopView.xaml
    /// </summary>
    public partial class DeleteShopView : Window
    {
        public DeleteShopView(DeleteShopViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }
    }
}
