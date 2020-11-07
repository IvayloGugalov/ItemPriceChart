using System;
using System.Collections.Generic;
using System.Linq;

using ItemPriceCharts.Services.Data;
using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Services
{
    public class OnlineShopService : IOnlineShopService
    {
        private readonly IUnitOfWork unitOfWork;

        public OnlineShopService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public OnlineShopModel FindShop(int id) =>
            this.unitOfWork.OnlineShopRepository.FindAsync(id).Result ?? throw new Exception();

        public IEnumerable<OnlineShopModel> GetAllShops() =>
            this.unitOfWork.OnlineShopRepository.All().Result;

        public bool IsShopExisting(int shopId) =>
            this.unitOfWork.OnlineShopRepository.IsExisting(shopId).Result;

        internal bool IsShopExisting(string url) =>
            this.unitOfWork.OnlineShopRepository.All(filter: shop => shop.URL == url).Result.FirstOrDefault() != null;

        public void CreateShop(string shopURL, string shopTitle)
        {
            try
            {
                if (!this.IsShopExisting(shopURL))
                {
                    var newShop = new OnlineShopModel(
                        id: default,
                        url: shopURL,
                        title: shopTitle);

                    this.unitOfWork.OnlineShopRepository.Add(newShop);
                    this.unitOfWork.SaveChangesAsync();

                    Events.ShopAdded.Publish(newShop);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void UpdateShop(OnlineShopModel onlineShop)
        {
            try
            {
                if (this.IsShopExisting(onlineShop.Id))
                {
                    this.unitOfWork.OnlineShopRepository.Update(onlineShop);
                    this.unitOfWork.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void DeleteShop(OnlineShopModel onlineShop)
        {
            try
            {
                if (this.IsShopExisting(onlineShop.Id))
                {
                    this.unitOfWork.OnlineShopRepository.Delete(onlineShop);
                    this.unitOfWork.SaveChangesAsync();

                    Events.ShopDeleted.Publish(onlineShop);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
