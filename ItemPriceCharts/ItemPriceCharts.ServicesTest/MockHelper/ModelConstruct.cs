using ItemPriceCharts.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItemPriceCharts.ServicesTest.MockHelper
{
    public static class ModelConstruct
    {
        public static ItemModel ConstructItem(
            int id,
            OnlineShopModel onlineShop = null,
            ItemType itemType = ItemType.ComputerItem,
            string url = null,
            string title = null,
            string descritpion = null,
            double price = 0)
        {
            var item = new ItemModel(
                id: id,
                url: url,
                title: title,
                description: descritpion,
                price: price,
                onlineShop: onlineShop,
                type: itemType);

            return item;
        }

        public static OnlineShopModel ConstructOnlineShop(
            int id,
            string url = null,
            string title = null)
        {
            var onlineShop = new OnlineShopModel(
                id: id,
                url: url,
                title: title);

            return onlineShop;
        }

        public static ItemPrice ConstructItemPrice(
            int id,
            DateTime priceDate,
            double currentPrice,
            int itemId)
        {
            var itemPrice = new ItemPrice(
                id: id,
                priceDate: priceDate,
                currentPrice: currentPrice,
                itemId: itemId);

            return itemPrice;
        }
    }
}
