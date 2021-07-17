using System.Windows.Controls;
using System.Windows.Input;

namespace ItemPriceCharts.UI.WPF.Views.UserControls
{
    /// <summary>
    /// Interaction logic for ItemListingView.xaml
    /// </summary>
    public partial class ItemListingView : UserControl
    {
        public ItemListingView()
        {
            InitializeComponent();
        }

        private void ItemDataGrid_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGrid {SelectedItems: {Count: 1}} grid)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(grid.SelectedItem) as DataGridRow;
                grid.UnselectAllCells();
                //if (!row.IsMouseOver)
                //{
                    //row.IsSelected = false;

                //}
            }
        }
    }
}
