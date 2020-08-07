using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

using HtmlAgilityPack;

using ItemPriceCharts.Services.Constants;
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
            var newShop = new OnlineShopModel(
                    id: default,
                    url: shopURL,
                    title: shopTitle);
            var isCreated =  this.onlineShopService.AddShop(newShop) == DatabaseResult.Succes;

            if (isCreated)
            {
                Events.ShopAdded.Publish(newShop);
            }
        }

        public void CreateItem(string itemURL, OnlineShopModel onlineShop, ItemModel.ItemType type)
        {
            var itemDocument = this.htmlService.Load(itemURL);
            var item = RetrieveItemData.CreateModel(itemURL, itemDocument, onlineShop, type);
            this.itemService.AddItem(item);
            Events.ItemAdded.Publish(item);
        }

        public void DeleteShop(OnlineShopModel onlineShop)
        {
            this.onlineShopService.Delete(onlineShop.Id);
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
            return Task.Run(() => this.onlineShopService.IsShopExisting(onlineShop.Id)).Result;
        }

        public bool TryGetItem(ItemModel item)
        {
            return Task.Run(() => this.itemService.IsItemExisting(item)).Result;
        }
    }
}
