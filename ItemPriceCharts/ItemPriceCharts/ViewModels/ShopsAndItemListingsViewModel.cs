﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

using NLog;

using ItemPriceCharts.Services.Events;
using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.UI.WPF.CommandHelpers;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class ShopsAndItemListingsViewModel : BaseListingViewModel
    {
        private static readonly Logger logger = LogManager.GetLogger(nameof(ShopsAndItemListingsViewModel));

        private readonly IItemService itemService;
        private readonly IOnlineShopService onlineShopService;
        private OnlineShop selectedShop;
        private bool isListOfShopsShown;

        public OnlineShop SelectedShop
        {
            get => this.selectedShop;
            set => SetValue(ref this.selectedShop, value);
        }

        public bool IsListOfShopsShown
        {
            get => this.isListOfShopsShown;
            set => this.SetValue(ref this.isListOfShopsShown, value);
        }

        public ObservableCollection<OnlineShop> OnlineShops { get; set; }

        public ICommand ShowItemsCommand { get; }
        public ICommand ShowCreateShopCommand { get; }
        public ICommand ShowDeleteShopCommand { get; }
        public ICommand ShowAddItemCommand { get; }

        public ShopsAndItemListingsViewModel(ItemService itemService, OnlineShopService onlineShopService)
            : base (itemService)
        {
            this.itemService = itemService;
            this.onlineShopService = onlineShopService;

            this.ShowCreateShopCommand = new RelayCommand(_ => this.ShowCreateShopAction());
            this.ShowDeleteShopCommand = new RelayCommand(_ => this.ShowDeleteShopAction());
            this.ShowAddItemCommand = new RelayCommand(_ => this.ShowAddItemAction());
            this.ShowItemsCommand = new RelayCommand<object>(this.AddItemsForShopAction, this.ShowItemsPredicate);

            this.ShouldShowShopInformation = true;

            EventsLocator.ShopAdded.Subscribe(this.OnAddedShop);
            EventsLocator.ShopDeleted.Subscribe(this.OnDeletedShop);

            this.AddShopsToViewModel();
        }

        private void ShowCreateShopAction() => UIEvents.ShowCreateShopViewModel.Publish(null);
        private void ShowDeleteShopAction() => UIEvents.ShowDeleteShopViewModel.Publish(this.SelectedShop);
        private void ShowAddItemAction() => UIEvents.ShowCreateItemViewModel.Publish(this.SelectedShop);

        private void AddItemsForShopAction(object parameter)
        {
            try
            {
                //Don't make call if the items for the shop are already shown
                if (this.ItemsList != null && this.ItemsList.Count > 0 &&
                    this.ItemsList.FirstOrDefault().OnlineShop.Equals(this.SelectedShop))
                {
                    return;
                }

                this.ItemsList = ToObservableCollectionExtensions.ToObservableCollection(
                    this.itemService.GetItemsForShop(this.SelectedShop));

                if (this.ItemsList.Any())
                {
                    this.AreItemsShown = true;
                }
            }
            catch (System.Exception e)
            {
                logger.Info($"Can't load items for {this.SelectedShop}: {e}");
                UIEvents.ShowMessageDialog(
                    new MessageDialogViewModel(
                        "Error",
                        e.Message,
                        ButtonType.Close));
            }
        }

        private void OnAddedShop(object sender, OnlineShop e)
        {
            Dispatcher dispatcher = Application.Current.Dispatcher;
            dispatcher.Invoke(() =>
            {
                this.OnlineShops.Add(e);
                this.IsListOfShopsShown = true;
            });
        }

        private void OnDeletedShop(object sender, OnlineShop e)
        {
            Dispatcher dispatcher = Application.Current.Dispatcher;
            dispatcher.Invoke(() =>
            {
                this.OnlineShops.Remove(e);

                this.OnPropertyChanged(() => this.IsListOfShopsShown);
            });
        }

        private void AddShopsToViewModel()
        {
            try
            {
                this.OnlineShops = ToObservableCollectionExtensions.ToObservableCollection(
                this.onlineShopService.GetAllShops());

                this.IsListOfShopsShown = this.OnlineShops.Any();
            }
            catch (System.Exception e)
            {
                logger.Info($"Can't load shops: {e}");
            }
        }

        private bool ShowItemsPredicate()
        {
            return this.OnlineShops.Any();
        }
    }
}
