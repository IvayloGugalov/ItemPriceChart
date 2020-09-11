using System;
using System.Collections.Generic;

using HtmlAgilityPack;

using ItemPriceCharts.Services.Helpers;
using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Strategies;

namespace ItemPriceCharts.Services.Services
{
    public class WebPageService : IWebPageService
    {
        private readonly HtmlWeb htmlService = new HtmlWeb();
        private readonly IItemService itemService;
        private readonly IOnlineShopService onlineShopService;
        private readonly IItemPriceService itemPriceService;

        public WebPageService(ItemService itemService, OnlineShopService onlineShopService, ItemPriceService itemPriceService)
        {
            this.itemService = itemService;
            this.onlineShopService = onlineShopService;
            this.itemPriceService = itemPriceService;
        }

        public void CreateShop(string shopURL, string shopTitle)
        {
            var newShop = 
                new OnlineShopModel(
                    id: default,
                    url: shopURL,
                    title: shopTitle);
            var isCreated =  this.onlineShopService.AddShop(newShop) == DatabaseResult.Succes;

            if (isCreated)
            {
                Events.ShopAdded.Publish(newShop);
            }
        }

        public void CreateItem(string itemURL, OnlineShopModel onlineShop, ItemType type)
        {
            var itemDocument = this.htmlService.Load(itemURL);
            var item = RetrieveItemData.CreateModel(itemURL, itemDocument, onlineShop, type);
            this.itemService.AddItem(item);
            this.itemPriceService.CreateItemPrice(new ItemPrice(
                id: default,
                priceDate: DateTime.Now,
                currentPrice: item.CurrentPrice,
                itemId: item.Id));

            Events.ItemAdded.Publish(item);
        }

        public void DeleteShop(OnlineShopModel onlineShop)
        {
            this.onlineShopService.DeleteShop(onlineShop);
            Events.ShopDeleted.Publish(onlineShop);
        }

        public void DeleteItem(ItemModel item)
        {
            this.itemService.DeleteItem(item);
            Events.ItemDeleted.Publish(item);
        }

        public IEnumerable<OnlineShopModel> RetrieveOnlineShops()
        {
            return this.onlineShopService.GetAll();
        }

        public IEnumerable<ItemModel> RetrieveItemsForShop(OnlineShopModel onlineShop)
        {
            return this.itemService.GetAll(onlineShop);
        }

        public bool TryGetShop(OnlineShopModel onlineShop)
        {
            return this.onlineShopService.IsShopExisting(onlineShop.Id);
        }

        public bool TryGetItem(ItemModel item)
        {
            return this.itemService.IsItemExisting(item);
        }
    }
}
