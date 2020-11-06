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
            this.unitOfWork.ItemRepository.All(i => i.URL == item.URL || i.Id == item.Id).Result.FirstOrDefault() != null;

        public bool UpdateItemPrice(ItemModel item, out ItemPrice updatedItemPrice)
        {
            try
            {
                updatedItemPrice = null;
                if (this.TryGetItem(item, out var existingItem))
                {
                    var itemDocument = this.htmlService.Load(item.URL);
                    var updatedItem = RetrieveItemData.CreateModel(item.URL, itemDocument, item.OnlineShop, item.Type);

                    if (!existingItem.Equals(item))
                    {
                        item.Description = updatedItem.Description;
                        item.CurrentPrice = updatedItem.CurrentPrice;

                        this.UpdateItem(item);

                        updatedItemPrice = new ItemPrice(
                            id: default,
                            priceDate: DateTime.Now,
                            currentPrice: item.CurrentPrice,
                            itemId: item.Id);

                        this.itemPriceService.CreateItemPrice(updatedItemPrice);

                        return true;
                    }
                }

                return false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private bool TryGetItem(ItemModel item, out ItemModel existingItem)
        {
            existingItem = this.unitOfWork.ItemRepository.All(i => i.URL == item.URL || i.Id == item.Id).Result.FirstOrDefault();

            return existingItem != null;
        }
    }
}
