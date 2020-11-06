using System;
using System.Threading.Tasks;
using System.Windows.Input;

using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Helpers;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class CreateItemViewModel : BindableViewModel
    {
        private readonly IItemService itemService;
        private ItemType selectedItemType;
        private string newItemURL;

        public string NewItemURL
        {
            get => this.newItemURL;
            set => this.SetValue(ref this.newItemURL, value);
        }

        public ItemType SelectedItemType
        {
            get => this.selectedItemType;
            set => this.SetValue(ref this.selectedItemType, value);
        }

        public OnlineShopModel SelectedShop { get; }

        public ICommand AddItemCommand { get; }

        public CreateItemViewModel(ItemService itemService, OnlineShopModel selectedShop)
        {
            this.itemService = itemService;
            this.SelectedShop = selectedShop ?? throw new ArgumentNullException();

            this.AddItemCommand = new RelayCommand<object>(this.AddItemAction, this.AddItemPredicate);
        }

        private void AddItemAction(object obj)
        {
            try
            {
                Task.Run(() => this.itemService.CreateItem(this.NewItemURL, this.SelectedShop, this.SelectedItemType));
            }
            catch (Exception e)
            {
                UIEvents.ShowMessageDialog(
                        new MessageDialogViewModel(
                            title: "Error",
                            description: e.Message,
                            buttonType: ButtonType.Close));
            }
        }

        private bool AddItemPredicate()
        {
            return ValidateURL.IsValidAddress(this.NewItemURL) && 
                this.NewItemURL.ToLower().Contains(this.SelectedShop.Title.ToLower());
        }
    }
}
