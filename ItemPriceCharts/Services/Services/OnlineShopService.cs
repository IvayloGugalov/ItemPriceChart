using System;
using System.Collections.Generic;
using System.Linq;

using NLog;

using ItemPriceCharts.Services.Data;
using ItemPriceCharts.Services.Events;
using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Services
{
    public class OnlineShopService : IOnlineShopService
    {
        private static readonly Logger logger = LogManager.GetLogger(nameof(OnlineShopService));

        private readonly IRepository<OnlineShop> onlineShopRepository;

        public OnlineShopService(IRepository<OnlineShop> onlineShopRepository)
        {
            this.onlineShopRepository = onlineShopRepository;
        }

        public OnlineShop FindShop(int id) =>
            this.onlineShopRepository.FindAsync(id).Result ?? throw new Exception();

        public IEnumerable<OnlineShop> GetAllShops() =>
            this.onlineShopRepository.GetAll(includeProperties: "Items").Result;

        public bool IsShopExisting(int shopId) =>
            this.onlineShopRepository.IsExisting(shopId).Result;

        internal bool IsShopExisting(string url) =>
            this.onlineShopRepository.GetAll(filter: shop => shop.URL == url).Result.FirstOrDefault() != null;

        public void CreateShop(string shopURL, string shopTitle)
        {
            try
            {
                if (!this.IsShopExisting(shopURL))
                {
                    var newShop = new OnlineShop(
                        url: shopURL,
                        title: shopTitle);

                    this.onlineShopRepository.Add(newShop);
                    logger.Debug($"Created shop: '{newShop}'");

                    EventsLocator.ShopAdded.Publish(newShop);
                }
            }
            catch (Exception e)
            {
                logger.Error($"Can't create shop: {e}");
                throw;
            }
        }

        public void UpdateShop(OnlineShop onlineShop)
        {
            try
            {
                if (this.IsShopExisting(onlineShop.Id))
                {
                    this.onlineShopRepository.Update(onlineShop);
                    logger.Debug($"Updated shop: '{onlineShop}'");
                }
            }
            catch (Exception e)
            {
                logger.Error($"Can't update shop: {e}");
            }
        }

        public void DeleteShop(OnlineShop onlineShop)
        {
            try
            {
                if (this.IsShopExisting(onlineShop.Id))
                {
                    this.onlineShopRepository.Delete(onlineShop);
                    logger.Debug($"Deleted shop: '{onlineShop}'");

                    EventsLocator.ShopDeleted.Publish(onlineShop);
                }
            }
            catch (Exception e)
            {
                logger.Error($"Can't delete shop: '{onlineShop}': {e}");
            }
        }
    }
}
