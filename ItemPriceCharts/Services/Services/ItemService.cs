using System;
using System.Collections.Generic;
using System.Linq;

using HtmlAgilityPack;

using ItemPriceCharts.Services.Data;
using ItemPriceCharts.Services.Helpers;
using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Services
{
    public class ItemService : IItemService
    {
        private readonly HtmlWeb htmlService = new HtmlWeb();
        private readonly IUnitOfWork unitOfWork;
        private readonly IItemPriceService itemPriceService;

        public ItemService(UnitOfWork unitOfWork, ItemPriceService itemPriceService)
        {
            this.unitOfWork = unitOfWork;
            this.itemPriceService = itemPriceService;
        }

        public ItemModel GetById(int id) =>
            this.unitOfWork.ItemRepository.All(item => item.Id == id).Result
                .FirstOrDefault() ?? throw new Exception();

        public void CreateItem(string itemURL, OnlineShopModel onlineShop, ItemType type)
        {
            try
            {
                var itemDocument = this.htmlService.Load(itemURL);
                var item = RetrieveItemData.CreateModel(itemURL, itemDocument, onlineShop, type);

                if (!this.IsItemExisting(item))
                {
                    this.unitOfWork.ItemRepository.Add(item);
                    this.unitOfWork.SaveChanges();

                    this.itemPriceService.CreateItemPrice(new ItemPrice(
                        id: default,
                        priceDate: DateTime.Now,
                        currentPrice: item.CurrentPrice,
                        itemId: item.Id));

                    Events.ItemAdded.Publish(item);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateItem(ItemModel item)
        {
            try
            {
                if (this.IsItemExisting(item))
                {
                    this.unitOfWork.ItemRepository.Update(item);
                    this.unitOfWork.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void DeleteItem(ItemModel item)
        {
            try
            {
                if (this.IsItemExisting(item))
                {
                    this.unitOfWork.ItemRepository.Delete(item);
                    this.unitOfWork.SaveChanges();

                    Events.ItemDeleted.Publish(item);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<ItemModel> GetAll(OnlineShopModel onlineShop) =>
            this.unitOfWork.ItemRepository.All(filter: item => item.OnlineShop.Id == onlineShop.Id).Result;

        public bool IsItemExisting(ItemModel item) =>
            this.unitOfWork.ItemRepository.All(i => i.URL == item.URL || i.Id == item.Id).Result.Any();
    }
}
