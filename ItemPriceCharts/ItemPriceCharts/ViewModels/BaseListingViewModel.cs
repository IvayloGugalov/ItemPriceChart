﻿using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

using ItemPriceCharts.Services.Models;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Helpers;

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

        public BaseListingViewModel(UserAccount userAccount)
        {
            this.UserAccount = userAccount ?? throw new ArgumentNullException(nameof(userAccount));

            this.ShowItemInformationDialogCommand = new RelayCommand(_ => this.ShowItemInformationDialogAction());
            this.DeleteItemCommand = new RelayCommand(_ => this.DeleteItemAction());

            UIEvents.ItemAdded.Subscribe(this.AddItemToItemsListHandler);
            UIEvents.ItemDeleted.Subscribe(this.RemoveItemFromItemsListHandler);
        }

        private void ShowItemInformationDialogAction() => UIEvents.ShowItemInformatioView.Publish(this.SelectedItem);

        private void DeleteItemAction() => UIEvents.ShowDeleteItemView.Publish(this.SelectedItem);

        private void AddItemToItemsListHandler(object sender, Item e)
        {
            if (this.ItemsList is not null)
            {
                this.ItemsList.Add(e);
                this.AreItemsShown = true;
            }
        }

        private void RemoveItemFromItemsListHandler(object sender, Item e)
        {
            if (this.ItemsList is not null)
            {
                this.ItemsList.Remove(e);
            }
        }
    }
}
