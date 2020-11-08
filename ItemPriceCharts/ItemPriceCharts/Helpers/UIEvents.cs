using System;

using ItemPriceCharts.Services.Events;
using ItemPriceCharts.Services.Models;
using ItemPriceCharts.UI.WPF.ViewModels;

namespace ItemPriceCharts.UI.WPF.Helpers
{
    public static class UIEvents
    {
        public static IChannel<object> ShowCreateShopViewModel { get; set; } = new Channel<object>();
        public static IChannel<OnlineShopModel> ShowDeleteShopViewModel { get; set; } = new Channel<OnlineShopModel>();
        public static IChannel<OnlineShopModel> ShowCreateItemViewModel { get; set; } = new Channel<OnlineShopModel>();
        public static IChannel<ItemModel> ShowItemInformatioViewModel { get; set; } = new Channel<ItemModel>();
        public static Func<MessageDialogViewModel, bool?> ShowMessageDialog { get; set; }
    }
}