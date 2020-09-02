using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Helpers;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class CreateItemViewModel : BindableViewModel
    {
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
            this.SelectedShop = selectedShop ?? throw new ArgumentNullException();

            this.AddItemCommand = new RelayCommand<object>(this.AddItemAction, this.AddItemPredicate);
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
            return ValidateURL.IsValidAddress(this.NewItemURL) && 
                this.NewItemURL.ToLower().Contains(this.SelectedShop.Title.ToLower());
        }
    }
}
