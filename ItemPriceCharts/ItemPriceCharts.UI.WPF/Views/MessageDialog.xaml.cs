using System;

using ControlzEx.Theming;
using MahApps.Metro.Controls;

using ItemPriceCharts.UI.WPF.ViewModels;

namespace ItemPriceCharts.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for MessageDialog.xaml
    /// </summary>
    public partial class MessageDialog : MetroWindow
    {
        public MessageDialog(MessageDialogViewModel viewModel)
        {
            this.DataContext = viewModel ?? throw new ArgumentNullException();
            InitializeComponent();

            if (viewModel.CloseWindow == null)
            {
                viewModel.CloseWindow = this.Close;
            }

            this.negativeButton.Click += (s, e) => this.Close();
            this.positiveButton.Click += (s, e) =>
            {
                this.DialogResult = true;
                this.Close();
            };

            ThemeManager.Current.ChangeTheme(this, "Light.Crimson");
        }
    }
}
