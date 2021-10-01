using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItemPriceCharts.UI.WPF.Views.UserControls
{
    /// <summary>
    /// Interaction logic for UserSettingsModalControl.xaml
    /// </summary>
    public partial class UserSettingsModalControl : UserControl
    {
        public UserSettingsModalControl()
        {
            InitializeComponent();
           
        }

        private void ShowPasswordCharsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            this.currentPassword.Visibility = System.Windows.Visibility.Collapsed;
            this.currentPasswordTextBox.Visibility = System.Windows.Visibility.Visible;

            this.currentPasswordTextBox.Focus();
        }

        private void ShowPasswordCharsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            this.currentPasswordTextBox.Visibility = System.Windows.Visibility.Collapsed;
            this.currentPassword.Visibility = System.Windows.Visibility.Visible;

            this.currentPassword.Focus();
        }
    }
}
