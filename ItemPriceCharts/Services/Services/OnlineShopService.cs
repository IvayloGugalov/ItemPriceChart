using System;
using System.Collections.Generic;
using System.Linq;

using ItemPriceCharts.Services.Data;
using ItemPriceCharts.Services.Events;
using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Services
{
    public class OnlineShopService : IOnlineShopService
    {
        private readonly IModelsContext modelsContext;

        public OnlineShopService(ModelsContext modelsContext)
        {
            this.modelsContext = modelsContext;
        }

        public OnlineShop FindShop(int id) =>
            this.modelsContext.OnlineShopRepository.FindAsync(id).Result ?? throw new Exception();

        public IEnumerable<OnlineShop> GetAllShops() =>
            this.modelsContext.OnlineShopRepository.All().Result;

        public bool IsShopExisting(int shopId) =>
            this.modelsContext.OnlineShopRepository.IsExisting(shopId).Result;

        internal bool IsShopExisting(string url) =>
            this.modelsContext.OnlineShopRepository.All(filter: shop => shop.URL == url).Result.FirstOrDefault() != null;

        public void CreateShop(string shopURL, string shopTitle)
        {
            try
            {
                if (!this.IsShopExisting(shopURL))
                {
                    var newShop = new OnlineShop(
                        url: shopURL,
                        title: shopTitle);

                    this.modelsContext.OnlineShopRepository.Add(newShop);
                    this.modelsContext.CommitChangesAsync();

                    EventsLocator.ShopAdded.Publish(newShop);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateShop(OnlineShop onlineShop)
        {
            try
            {
                if (this.IsShopExisting(onlineShop.Id))
                {
                    this.modelsContext.OnlineShopRepository.Update(onlineShop);
                    this.modelsContext.CommitChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void DeleteShop(OnlineShop onlineShop)
        {
            try
            {
                if (this.IsShopExisting(onlineShop.Id))
                {
                    this.modelsContext.OnlineShopRepository.Delete(onlineShop);
                    this.modelsContext.CommitChangesAsync();

                    EventsLocator.ShopDeleted.Publish(onlineShop);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
