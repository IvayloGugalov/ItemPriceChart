using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            this.onlineShopRepository.FindAsync(id).GetAwaiter().GetResult() ?? throw new Exception();

        public Task<IEnumerable<OnlineShop>> GetAllShops() =>
            this.onlineShopRepository.GetAll(includeProperties: "Items");

        public bool IsShopExisting(int shopId) =>
            this.onlineShopRepository.IsExisting(shopId).GetAwaiter().GetResult();

        private bool IsShopExisting(string url) =>
            this.onlineShopRepository.GetAll(filter: shop => shop.URL == url).GetAwaiter().GetResult().FirstOrDefault() != null;

        public async Task CreateShop(string shopURL, string shopTitle)
        {
            try
            {
                if (!this.IsShopExisting(shopURL))
                {
                    var newShop = new OnlineShop(
                        url: shopURL,
                        title: shopTitle);

                    var onlineShop = await this.onlineShopRepository.Add(newShop);
                    logger.Debug($"Created shop: '{onlineShop}'.");

                    EventsLocator.ShopAdded.Publish(onlineShop);
                }
            }
            catch (Exception e)
            {
                logger.Error(e, "Can't create shop.");
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
                    logger.Debug($"Updated shop: '{onlineShop}'.");
                }
            }
            catch (Exception e)
            {
                logger.Error(e, "Can't update shop.");
            }
        }

        public void DeleteShop(OnlineShop onlineShop)
        {
            try
            {
                if (this.IsShopExisting(onlineShop.Id))
                {
                    this.onlineShopRepository.Delete(onlineShop);
                    logger.Debug($"Deleted shop: '{onlineShop}'.");

                    EventsLocator.ShopDeleted.Publish(onlineShop);
                }
            }
            catch (Exception e)
            {
                logger.Error(e, $"Can't delete shop: '{onlineShop}'.");
            }
        }
    }
}
