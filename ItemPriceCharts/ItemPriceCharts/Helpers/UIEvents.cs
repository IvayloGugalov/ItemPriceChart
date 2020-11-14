using System;

using ItemPriceCharts.Services.Events;
using ItemPriceCharts.Services.Models;
using ItemPriceCharts.UI.WPF.ViewModels;

namespace ItemPriceCharts.UI.WPF.Helpers
{
    public static class UIEvents
    {
        public static IChannel<object> ShowCreateShopViewModel { get; set; } = new Channel<object>();
        public static IChannel<OnlineShop> ShowDeleteShopViewModel { get; set; } = new Channel<OnlineShop>();
        public static IChannel<OnlineShop> ShowCreateItemViewModel { get; set; } = new Channel<OnlineShop>();
        public static IChannel<Item> ShowItemInformatioViewModel { get; set; } = new Channel<Item>();
        public static Func<MessageDialogViewModel, bool?> ShowMessageDialog { get; set; }
    }
}