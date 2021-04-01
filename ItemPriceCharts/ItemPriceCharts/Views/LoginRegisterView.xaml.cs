using System;
using System.Windows;

using ItemPriceCharts.UI.WPF.ViewModels.LoginAndRegistration;

namespace ItemPriceCharts.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for LoginRegisterView.xaml
    /// </summary>
    public partial class LoginRegisterView : Window
    {
        public LoginRegisterView(LoginViewModel viewModel)
        {
            this.DataContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

            InitializeComponent();
        }
    }
}
