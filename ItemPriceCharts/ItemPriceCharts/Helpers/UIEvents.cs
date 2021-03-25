using System;
using System.Collections.Generic;
using System.Windows;

using ItemPriceCharts.Services.Events;
using ItemPriceCharts.Services.Models;
using ItemPriceCharts.UI.WPF.ViewModels;
using ItemPriceCharts.UI.WPF.ViewModels.LoginAndRegistration;

namespace ItemPriceCharts.UI.WPF.Helpers
{
    public static class UIEvents
    {
        private static List<Action> subscribers = null;

        public static Func<MessageDialogViewModel, bool?> ShowMessageDialog { get; set; }
        public static Func<LoginViewModel, bool?> ShowLoginRegisterWindow { get; set; }

        public static void CloseApplication() => Application.Current.Dispatcher.Invoke(Application.Current.Shutdown);

        //Directly called on the UI thread
        public static IChannel<object> ShowCreateShopView { get; set; } = new Channel<object>();
        public static IChannel<OnlineShop> ShowCreateItemView { get; set; } = new Channel<OnlineShop>();
        public static IChannel<Item> ShowDeleteItemView { get; set; } = new Channel<Item>();
        public static IChannel<Item> ShowItemInformatioView { get; set; } = new Channel<Item>();

        //Events from EventsLocator for the UI
        public static IChannel<Item> ItemAdded { get; set; } = new Channel<Item>();
        public static IChannel<Item> ItemDeleted { get; set; } = new Channel<Item>();
        public static IChannel<OnlineShop> ShopAdded { get; set; } = new Channel<OnlineShop>();
        public static IChannel<OnlineShop> ShopDeleted { get; set; } = new Channel<OnlineShop>();

        public static void AddSubscribers()
        {
            subscribers = new List<Action>
            {
                new UISubscription<Item>(EventsLocator.ItemAdded, UIEvents.ItemAdded).Replay,

                new UISubscription<Item>(EventsLocator.ItemDeleted, UIEvents.ItemDeleted).Replay,

                new UISubscription<OnlineShop>(EventsLocator.ShopAdded, UIEvents.ShopAdded).Replay,

                new UISubscription<OnlineShop>(EventsLocator.ShopDeleted, UIEvents.ShopDeleted).Replay
            };
        }

        public static void FinishSubscribing()
        {
            if (null == subscribers)
            {
                throw new Exception();
            }
        }

        private class UISubscription<TMessage>
        {
            private readonly Action<TMessage> handler;

            public UISubscription(IChannel<TMessage> channel, Action<TMessage> handler)
            {
                this.handler = handler;
                channel.Subscribe(this.PassMessage);
            }

            public UISubscription(IChannel<TMessage> channel, IChannel<TMessage> uiChannel)
                : this(channel, message => uiChannel.Publish(message))
            { }

            public void Replay()
            { }

            private void PassMessage(object sender, TMessage message)
            {
                Application.Current.Dispatcher.InvokeAsync(() => this.handler(message));
            }
        }
    }
}