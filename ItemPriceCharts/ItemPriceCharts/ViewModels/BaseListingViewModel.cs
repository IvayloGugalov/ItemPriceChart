﻿using System.Collections.ObjectModel;
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
    public class BaseListingViewModel : BindableViewModel
    {
        private readonly IItemService itemService;
        private ObservableCollection<Item> itemsList;
        private Item selectedItem;
        private bool areItemsShown;
        private bool shouldShowShopInformation;

        public ObservableCollection<Item> ItemsList
        {
            get => this.itemsList;
            set => this.SetValue(ref this.itemsList, value);
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

        public BaseListingViewModel(ItemService itemService)
        {
            this.itemService = itemService;

            this.ShowItemInformationDialogCommand = new RelayCommand(_ => this.ShowItemInformationDialogAction());
            this.DeleteItemCommand = new RelayCommand(_ => this.DeleteItemAction());

            EventsLocator.ItemAdded.Subscribe(this.AddItemToItemsListHandler);
            EventsLocator.ItemDeleted.Subscribe(this.RemoveItemFromItemsListHandler);
        }

        private void ShowItemInformationDialogAction() => UIEvents.ShowItemInformatioViewModel.Publish(this.SelectedItem);
        private void DeleteItemAction() => this.itemService.DeleteItem(this.SelectedItem);

        private void AddItemToItemsListHandler(object sender, Item e)
        {
            Dispatcher dispatcher = Application.Current.Dispatcher;
            dispatcher.Invoke(() =>
            {
                this.ItemsList.Add(e);
                this.AreItemsShown = true;
                this.OnPropertyChanged(() => this.AreItemsShown);
            });
        }

        private void RemoveItemFromItemsListHandler(object sender, Item e)
        {
            Dispatcher dispatcher = Application.Current.Dispatcher;
            dispatcher.Invoke(() =>
            {
                this.ItemsList.Remove(e);
            });
        }
    }
}
