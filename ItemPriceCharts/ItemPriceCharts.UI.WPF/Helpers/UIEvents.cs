using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading;
using ControlzEx.Standard;
using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.Domain.Events;
using ItemPriceCharts.UI.WPF.ViewModels;
using ItemPriceCharts.UI.WPF.ViewModels.LoginAndRegistration;

namespace ItemPriceCharts.UI.WPF.Helpers
{
    public static class UiEvents
    {

        public static Func<MessageDialogViewModel, bool?> ShowMessageDialog { get; set; }
        public static Func<LoginViewModel, bool?> ShowLoginRegisterWindow { get; set; }

        public static void CloseApplication() => Application.Current.Dispatcher.Invoke(Application.Current.Shutdown);

        //Directly called on the UI thread
        public static DomainEvent<UserAccount> ShowCreateShopView { get; set; } = new();
        public static DomainEvent<OnlineShop> ShowCreateItemView { get; set; } = new();
        public static DomainEvent<Item> ShowDeleteItemView { get; set; } = new();
        public static DomainEvent<Item> ShowItemInformationView { get; set; } = new();

        //Events from EventsLocator for the UI
        public static IDomainEventWrapper<Item> ItemAdded { get; set; } = new DomainEventWrapper<Item>(DomainEvents.ItemAdded);
        public static IDomainEventWrapper<Item> ItemDeleted { get; set; } = new DomainEventWrapper<Item>(DomainEvents.ItemDeleted);
        public static IDomainEventWrapper<OnlineShop> ShopAdded { get; set; } = new DomainEventWrapper<OnlineShop>(DomainEvents.ShopAdded);
        public static IDomainEventWrapper<OnlineShop> ShopDeleted { get; set; } = new DomainEventWrapper<OnlineShop>(DomainEvents.ShopDeleted);
    }

    public class DomainEventWrapper<T> : IDomainEventWrapper<T>
    {
        private readonly IDomainEvent<T> domainEvent;

        public DomainEventWrapper(IDomainEvent<T> domainEvent = null)
        {
            this.domainEvent = domainEvent ?? new DomainEvent<T>();
        }

        public void Register(Action<T> action) => this.domainEvent.Register(action);
        
        public void Raise(T param) => Application.Current.Dispatcher.InvokeAsync(() => this.domainEvent.Raise(param));

        public void ClearHandlers() => this.domainEvent.ClearHandlers();
    }

    public interface IDomainEventWrapper<T> : IDomainEvent<T> {}
}