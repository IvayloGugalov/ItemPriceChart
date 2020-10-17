using System;
using System.Collections.Generic;

using HtmlAgilityPack;

using ItemPriceCharts.Services.Data;
using ItemPriceCharts.Services.Helpers;
using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Services
{
    public class ItemService : IItemService
    {
        private readonly HtmlWeb htmlService;
        private readonly IUnitOfWork unitOfWork;
        private readonly IItemPriceService itemPriceService;

        public ItemService(UnitOfWork unitOfWork, ItemPriceService itemPriceService)
        {
            this.unitOfWork = unitOfWork;
            this.itemPriceService = itemPriceService;
            this.htmlService = new HtmlWeb();
        }

        public ItemModel GetBy(object id) =>
            this.unitOfWork.ItemRepository.GetBy(id).Result ?? throw new Exception();

        public void CreateItem(string itemURL, OnlineShopModel onlineShop, ItemType type)
        {
            try
            {
                if (!this.IsItemExisting(itemURL))
                {
                    var itemDocument = this.htmlService.Load(itemURL);
                    var item = RetrieveItemData.CreateModel(itemURL, itemDocument, onlineShop, type);

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
                if (this.IsItemExisting(item.Id))
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
                if (this.IsItemExisting(item.Id))
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

        public bool IsItemExisting(object id) =>
            this.unitOfWork.ItemRepository.GetBy(id) != null;

        public ItemPrice UpdateItemPrice(ItemModel item)
        {
            try
            {
                if (this.IsItemExisting(item.Id))
                {
                    var itemDocument = this.htmlService.Load(item.URL);
                    var updatedItem = RetrieveItemData.CreateModel(item.URL, itemDocument, item.OnlineShop, item.Type);

                    item.Description = updatedItem.Description;
                    item.CurrentPrice = updatedItem.CurrentPrice;

                    this.UpdateItem(item);

                    var newestItemPrice = new ItemPrice(
                        id: default,
                        priceDate: DateTime.Now,
                        currentPrice: item.CurrentPrice,
                        itemId: item.Id);

                    this.itemPriceService.CreateItemPrice(newestItemPrice);

                    return newestItemPrice;
                }

                return null;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
