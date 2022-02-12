using System;
using System.Windows;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.Domain.Events;
using ItemPriceCharts.UI.WPF.Factories;
using ItemPriceCharts.UI.WPF.ViewModels;

namespace ItemPriceCharts.UI.WPF.Events
{
    public class UiEvents
    {
        public static Func<MessageDialogViewModel, bool?> ShowMessageDialog { get; set; }

        public static void CloseApplication() => Application.Current.Dispatcher.Invoke(Application.Current.Shutdown);

        //Directly called on the UI thread
        public static IEvent<UserAccount> ShowCreateShopView { get; set; } = new Event<UserAccount>();
        public static IEvent<OnlineShop> ShowCreateItemView { get; set; } = new Event<OnlineShop>();
        public static IEvent<Item> ShowDeleteItemView { get; set; } = new Event<Item>();
        public static IEvent<Item> ShowItemInformationView { get; set; } = new Event<Item>();
        public static IEvent<UserAccount> SuccessfulLogin { get; set; } = new Event<UserAccount>();
        public static IEvent<UserAccount> ShowUpdateEmailView { get; set; } = new Event<UserAccount>();
        public static IEvent<UserAccount> UserAccountUpdated { get; set; } = new Event<UserAccount>();


        //Events from EventsLocator for the UI
        public IUiEvent<Item> ItemAdded { get; set; }
        public IUiEvent<Item> ItemDeleted { get; set; }
        public IUiEvent<OnlineShop> ShopAdded { get; set; }
        public IUiEvent<OnlineShop> ShopDeleted { get; set; }

        public UiEvents(IDispatcherWrapper dispatcherWrapper)
        {
            this.ItemAdded = new UiEvent<Item>(DomainEvents.ItemAdded, dispatcherWrapper);
            this.ItemDeleted = new UiEvent<Item>(DomainEvents.ItemDeleted, dispatcherWrapper);
            this.ShopAdded = new UiEvent<OnlineShop>(DomainEvents.ShopAdded, dispatcherWrapper);
            this.ShopDeleted = new UiEvent<OnlineShop>(DomainEvents.ShopDeleted, dispatcherWrapper);
        }
    }
}