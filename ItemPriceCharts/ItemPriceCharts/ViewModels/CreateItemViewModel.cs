using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Views;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class CreateItemViewModel : BindableViewModel
    {
        //private readonly CreateItemView view;
        private readonly WebPageService webPageService;
        private string newItemURL;

        public string NewItemURL
        {
            get => this.newItemURL;
            set => this.SetValue(ref this.newItemURL, value);
        }

        public ICommand AddItemCommand { get; }

        public OnlineShopModel SelectedShop { get; }
        public ItemModel.ItemType ItemType { get; }

        public CreateItemViewModel(WebPageService webPageService, OnlineShopModel selectedShop)
        {
            this.webPageService = webPageService;
            this.SelectedShop = selectedShop;

            this.AddItemCommand = new RelayCommand<object>(this.AddItemAction, this.AddItemPredicate);

            //this.view = new CreateItemView(this);
            //view.ShowDialog();
        }

        private void AddItemAction(object obj)
        {
            try
            {
                Task.Run(() => this.webPageService.CreateItem(this.NewItemURL, this.SelectedShop, this.ItemType));
            }
            catch (Exception e)
            {
                MessageBox.Show($"We had an error: {e.Message}");
            }
        }

        private bool AddItemPredicate()
        {
            return this.NewItemURL != null && this.SelectedShop != null &&
                this.NewItemURL.ToLower().Contains(this.SelectedShop.Title.ToLower());
        }
    }
}
