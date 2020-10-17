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

        public OnlineShopModel GetBy(object id) =>
            this.unitOfWork.OnlineShopRepository.GetBy(id).Result ?? throw new Exception();

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
                    this.unitOfWork.SaveChanges();

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
                    this.unitOfWork.SaveChanges();
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
                    this.unitOfWork.SaveChanges();

                    Events.ShopDeleted.Publish(onlineShop);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<OnlineShopModel> GetAll() =>
            this.unitOfWork.OnlineShopRepository.All().Result;

        public bool IsShopExisting(object shopId) =>
            this.unitOfWork.OnlineShopRepository.GetBy(shopId) != null;
    }
}
