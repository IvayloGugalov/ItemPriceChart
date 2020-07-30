

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class ItemViewModel : BindableViewModel
    {
        private string title;
        private string price;

        public string Title
        {
            get => this.title;
            set => this.SetValue(ref this.title, value);
        }
        public string Price
        {
            get => this.price;
            set => this.SetValue(ref this.price, value);
        }

        public ItemViewModel(ItemModel item)
        {
            this.Title = item.Title;
            this.Price = item.Price.ToString();
        }
    }
}
