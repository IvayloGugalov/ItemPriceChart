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

        public WebPageService(ItemService itemService, OnlineShopService onlineShopService)
        {
            this.itemService = itemService;
            this.onlineShopService = onlineShopService;
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
