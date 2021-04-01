using ControlzEx.Theming;
using MahApps.Metro.Controls;

namespace ItemPriceCharts.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for DeleteItemView.xaml
    /// </summary>
    public partial class DeleteItemView : MetroWindow
    {
        public DeleteItemView()
        {
            InitializeComponent();

            this.negativeButton.Click += (s, e) => this.Close();
            this.positiveButton.Click += (s, e) => this.Close();

            ThemeManager.Current.ChangeTheme(this, "Light.Crimson");
        }
    }
}
