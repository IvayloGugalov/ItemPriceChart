

using Services.Models.Entities;
using UI.WPF.ViewModels;

namespace ItemPriceCharts.ViewModels
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

        public ItemViewModel(ItemResult item)
        {
            item.Title = this.title;
            item.Price = this.price;
        }

        private void Initizialize()
        {

        }
    }
}
