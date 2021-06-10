using MahApps.Metro.Controls;

namespace ItemPriceCharts.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for CreateShopView.xaml
    /// </summary>
    public partial class CreateShopView : MetroWindow
    {
        public CreateShopView()
        {
            InitializeComponent();

            this.createShop.Click += (_, _) => this.Close();
        }
    }
}
