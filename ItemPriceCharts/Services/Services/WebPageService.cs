using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

using HtmlAgilityPack;

using ItemPriceCharts.Services.Constants;
using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Strategies;

namespace ItemPriceCharts.Services.Services
{
    public class WebPageService : IWebPageService
    {
        private readonly int priceStartIndex = 26;
        private readonly int nameStartIndex = 25;
        private readonly int descriptionStartIndex = 29;
        private readonly string priceContent = @"itemprop=""price"" content=""";
        private readonly string titleContent = @"itemprop=""name"" content=""";
        private readonly string descriptionContent = @"itemprop=""description""";

        private readonly HtmlWeb htmlService = new HtmlWeb();
        private readonly IItemService itemService;
        private readonly IOnlineShopService onlineShopService;

        private readonly URLConstants urlConstants = new URLConstants();

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
                this.PublishEvent(Events.ShopAdded, newShop);
            }
        }

        public void CreateItem(string itemURL, OnlineShopModel onlineShop)
        {
            var itemDocument = this.htmlService.Load(itemURL);
            _ = double.TryParse(this.GetItemInformation(itemDocument, this.priceContent, this.priceStartIndex), out var price);

            var newItem = new ItemModel(
                    id: default,
                    url: itemURL,
                    title: this.GetItemInformation(itemDocument, this.titleContent, this.nameStartIndex),
                    description: this.GetItemInformation(itemDocument, this.descriptionContent, this.descriptionStartIndex),
                    price: price,
                    onlineShop: onlineShop);

            this.itemService.AddItem(newItem);
        }

        public void DeleteShop(OnlineShopModel onlineShop)
        {
            this.onlineShopService.Delete(onlineShop.Id);
            this.PublishEvent(Events.ShopDeleted, onlineShop);
        }

        public IEnumerable<ItemModel> RetrieveItemsForShop()
        {
            return this.itemService.GetAll();
        }

        public IEnumerable<OnlineShopModel> RetrieveOnlineShops()
        {
            return this.onlineShopService.GetAll();
        }

        public bool TryGetShop(OnlineShopModel onlineShop)
        {
            return this.onlineShopService.IsShopExisting(onlineShop.Id);
        }

        private void PublishEvent(IChannel<OnlineShopModel> channel, OnlineShopModel onlineShop)
        {
            channel.Publish(onlineShop);
        }

        private string GetItemInformation(HtmlDocument itemDocument, string itemContent, int startIndex)
        {
            var a = this.CycleForWord(itemDocument.Text.IndexOf(itemContent) + startIndex, itemDocument);
            return a;
        }

        private string CycleForWord(int index, HtmlDocument itemDocument)
        {
            var word = string.Empty;
            for (int i = index; i < itemDocument.Text.Length; i++)
            {
                if (itemDocument.Text[i] == '"')
                {
                    break;
                }
                word += itemDocument.Text[i];
            }
            return word;
        }
    }
}
