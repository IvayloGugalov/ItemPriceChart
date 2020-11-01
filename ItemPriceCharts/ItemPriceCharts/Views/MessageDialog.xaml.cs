using System;
using System.Windows;

using ItemPriceCharts.UI.WPF.ViewModels;

namespace ItemPriceCharts.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for MessageDialog.xaml
    /// </summary>
    public partial class MessageDialog : Window
    {
        public MessageDialog(MessageDialogViewModel viewModel)
        {
            this.DataContext = viewModel ?? throw new ArgumentNullException();
            InitializeComponent();

            if (viewModel.CloseWindow == null)
            {
                viewModel.CloseWindow = new Action(this.Close);
            }

            this.negativeButton.Click += (s, e) => this.Close();
            this.positiveButton.Click += (s, e) =>
            {
                this.DialogResult = true;
                this.Close();
            };
        }
    }
}
