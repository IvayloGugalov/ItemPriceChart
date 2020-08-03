using ItemPriceCharts.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItemPriceCharts.Services.Services
{
    public static class Events
    {
        public static IChannel<OnlineShopModel> ShopAdded { get; set; } = new Channel<OnlineShopModel>();
        public static IChannel<OnlineShopModel> ShopDeleted { get; set; } = new Channel<OnlineShopModel>();
    }
}
