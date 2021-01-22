using MahApps.Metro.Controls;

namespace ItemPriceCharts.UI.WPF.Views
{
    /// <summary>
    /// Interaction logic for CreateItemView.xaml
    /// </summary>
    public partial class CreateItemView : MetroWindow
    {
        public CreateItemView()
        {
            InitializeComponent();

            this.createItem.Click += (s, e) => this.Close();
        }
    }
}
