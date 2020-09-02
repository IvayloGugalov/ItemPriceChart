using System;

using ItemPriceCharts.Services.Services;

namespace ItemPriceCharts.UI.WPF.Helpers
{
    public static class UIEvents
    {
        public static IChannel<object> ShowCreateShopViewModel { get; set; } = new Channel<object>();
        public static IChannel<object> ShowDeleteShopViewModel { get; set; } = new Channel<object>();
        public static IChannel<object> ShowCreateItemViewModel { get; set; } = new Channel<object>();
    }
}
