using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using HtmlAgilityPack;
using Services.Constants;
using Services.Models;
using Services.Models.Entities;
using Services.Strategies;

namespace Services.Services
{
    public class WebPageService : IWebPageService
    {
        private readonly int priceStartIndex = 26;
        private readonly int nameStartIndex = 25;
        private readonly int descriptionStartIndex = 31;
        private readonly string priceContent = @"itemprop=""price"" content=""";
        private readonly string titleContent = @"itemprop=""name"" content=""";
        private readonly string descriptionContent = @"itemprop=""description""";

        private readonly HtmlWeb htmlService = new HtmlWeb();
        private readonly IEnumerable<ItemModel> items = new List<ItemModel>();
        private readonly IItemService itemService;
        private readonly IOnlineShopService onlineShopService;

        private readonly URLConstants urlConstants = new URLConstants();

        public WebPageService(ItemService itemService, OnlineShopService onlineShopService)
        {
            this.itemService = itemService;
            this.onlineShopService = onlineShopService;
            this.items = this.itemService.GetAll();
        }

        public void CreateShop(string shopURL, string shopTitle)
        {
            this.onlineShopService.Add(new OnlineShopModel()
            {
                Url = shopURL,
                Title = shopTitle
            });
        }

        public void CreateItem(string itemURL, int shopId)
        {
            var itemDocument = this.htmlService.Load(itemURL);
            _ = double.TryParse(this.GetItemInformation(itemDocument, this.priceContent, this.priceStartIndex), out var price);

            this.itemService.AddItem(new ItemModel()
            {
                URL = itemURL,
                OnlineShopModelId = shopId,
                Title = this.GetItemInformation(itemDocument, this.titleContent, this.nameStartIndex),
                Description = this.GetItemInformation(itemDocument, this.descriptionContent, this.descriptionStartIndex),
                Price = price
            });
        }

        public void DeleteShop(int id)
        {
            this.onlineShopService.Delete(id);
        }

        public IEnumerable<OnlineShopModel> RetrieveItemsForShop()
        {
            return this.onlineShopService.GetAll();
        }

        public IEnumerable<OnlineShopModel> RetrieveOnlineShops()
        {
            return this.onlineShopService.GetAll();
        }

        public List<ItemResult> FindRequiredTextForPC()
        {
            var listOfResults = new List<ItemResult>();

            foreach (var item in this.items)
            {
                var itemDocument = this.htmlService.Load(item.URL);

                listOfResults.Add(new ItemResult(
                    title: this.GetItemInformation(itemDocument, this.titleContent, this.nameStartIndex),
                    price: this.GetItemInformation(itemDocument, this.priceContent, this.priceStartIndex),
                    description: this.GetItemInformation(itemDocument, this.descriptionContent, this.descriptionStartIndex))
                );
            }

            return listOfResults;
        }

        private string GetItemInformation(HtmlDocument itemDocument, string itemContent, int startIndex)
        {
            return this.CycleForWord(itemDocument.Text.IndexOf(itemContent) + startIndex, itemDocument);
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
