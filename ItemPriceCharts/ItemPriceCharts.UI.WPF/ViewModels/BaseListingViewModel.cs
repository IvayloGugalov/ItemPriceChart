using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Events;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class BaseListingViewModel : BindableViewModel
    {
        private ObservableCollection<Item> itemsList;
        private OnlineShop selectedShop;
        private Item selectedItem;
        private bool areItemsShown;
        private bool shouldShowShopInformation;

        public UserAccount UserAccount { get; }

        public ObservableCollection<Item> ItemsList
        {
            get => this.itemsList;
            set => this.SetValue(ref this.itemsList, value);
        }

        public OnlineShop SelectedShop
        {
            get => this.selectedShop;
            set => SetValue(ref this.selectedShop, value);
        }

        public Item SelectedItem
        {
            get => this.selectedItem;
            set => SetValue(ref this.selectedItem, value);
        }

        public bool AreItemsShown
        {
            get => this.areItemsShown;
            set => this.SetValue(ref this.areItemsShown, value);
        }

        public bool ShouldShowShopInformation
        {
            get => this.shouldShowShopInformation;
            set => this.SetValue(ref this.shouldShowShopInformation, value);
        }

        public ICommand ShowItemInformationDialogCommand { get; }
        public ICommand DeleteItemCommand { get; }

        public BaseListingViewModel(UserAccount userAccount, UiEvents uiEvents)
        {
            this.UserAccount = userAccount ?? throw new ArgumentNullException(nameof(userAccount));

            this.ShowItemInformationDialogCommand = new RelayCommand(_ => this.ShowItemInformationDialogAction());
            this.DeleteItemCommand = new RelayCommand(_ => this.DeleteItemAction());

            uiEvents.ItemAdded.Register(this.AddItemToItemsListHandler);
            uiEvents.ItemDeleted.Register(this.RemoveItemFromItemsListHandler);
        }

        private void ShowItemInformationDialogAction() => UiEvents.ShowItemInformationView.Raise(this.SelectedItem);

        private void DeleteItemAction() => UiEvents.ShowDeleteItemView.Raise(this.SelectedItem);

        private void AddItemToItemsListHandler(Item e)
        {
            if (this.ItemsList is not null)
            {
                this.ItemsList.Add(e);
                this.AreItemsShown = true;
            }
        }

        private void RemoveItemFromItemsListHandler(Item e)
        {
            this.ItemsList?.Remove(e);
        }
    }
}
