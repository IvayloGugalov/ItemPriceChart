using System;
using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.Models;

namespace ItemPriceCharts.UI.WPF.Helpers
{
    public static class UIEvents
    {
        public static IChannel<object> ShowCreateShopViewModel { get; set; } = new Channel<object>();
        public static IChannel<OnlineShopModel> ShowDeleteShopViewModel { get; set; } = new Channel<OnlineShopModel>();
        public static IChannel<OnlineShopModel> ShowCreateItemViewModel { get; set; } = new Channel<OnlineShopModel>();
        public static IChannel<ItemModel> ShowItemInformatioViewModel { get; set; } = new Channel<ItemModel>();
        public static IChannel<Message> ShowMessageDialog { get; set; } = new Channel<Message>();
    }
}