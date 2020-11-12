using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

using ItemPriceCharts.Services.Events;
using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Helpers;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class ShopViewModel : BindableViewModel
    {
        private readonly IItemService itemService;
        private ObservableCollection<ItemModel> itemsList;
        private ItemModel selectedItem;
        private bool areItemsShown;
        private bool isListOfShopsShown;
        private bool shouldShowShopInformation;

        public ObservableCollection<ItemModel> ItemsList
        {
            get => this.itemsList;
            set => this.SetValue(ref this.itemsList, value);
        }

        public ObservableCollection<OnlineShopModel> OnlineShops { get; set; }

        public ItemModel SelectedItem
        {
            get => this.selectedItem;
            set => SetValue(ref this.selectedItem, value);
        }

        public bool AreItemsShown
        {
            get => this.areItemsShown;
            set => this.SetValue(ref this.areItemsShown, value);
        }

        public bool IsListOfShopsShown
        {
            get => this.isListOfShopsShown;
            set => this.SetValue(ref this.isListOfShopsShown, value);
        }

        public bool ShouldShowShopInformation
        {
            get => this.shouldShowShopInformation;
            set => this.SetValue(ref this.shouldShowShopInformation, value);
        }

        public ICommand ShowItemInformationDialogCommand { get; }
        public ICommand DeleteItemCommand { get; }

        public ShopViewModel(ItemService itemService)
        {
            this.itemService = itemService;

            this.ShowItemInformationDialogCommand = new RelayCommand(_ => this.ShowItemInformationDialogAction());
            this.DeleteItemCommand = new RelayCommand(_ => this.DeleteItemAction());

            EventsLocator.ItemAdded.Subscribe(this.AddItemToItemsListHandler);
            EventsLocator.ItemDeleted.Subscribe(this.RemoveItemFromItemsListHandler);
        }

        private void ShowItemInformationDialogAction() => UIEvents.ShowItemInformatioViewModel.Publish(this.SelectedItem);
        private void DeleteItemAction() => this.itemService.DeleteItem(this.SelectedItem);

        private void AddItemToItemsListHandler(object sender, ItemModel e)
        {
            Dispatcher dispatcher = Application.Current.Dispatcher;
            dispatcher.Invoke(() =>
            {
                this.ItemsList.Add(e);
            });
        }

        private void RemoveItemFromItemsListHandler(object sender, ItemModel e)
        {
            Dispatcher dispatcher = Application.Current.Dispatcher;
            dispatcher.Invoke(() =>
            {
                this.ItemsList.Remove(e);
            });
        }
    }
}
