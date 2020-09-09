using System;
using System.Collections.Generic;
using System.Linq;

using ItemPriceCharts.Services.Data;
using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;

namespace ItemPriceCharts.Services.Strategies
{
    public class OnlineShopService : IOnlineShopService
    {
        private readonly IUnitOfWork unitOfWork;

        public OnlineShopService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public OnlineShopModel GetById(int id) =>
            this.unitOfWork.OnlineShopRepository.All(shop => shop.Id == id).Result
                .FirstOrDefault() ?? throw new Exception();

        public DatabaseResult AddShop(OnlineShopModel onlineShop)
        {
            try
            {
                if (!this.IsShopExisting(onlineShop.URL))
                {
                    this.unitOfWork.OnlineShopRepository.Add(onlineShop);
                    this.unitOfWork.SaveChanges();

                    return DatabaseResult.Succes;
                }
                return DatabaseResult.EntitityAlreadyExist;
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
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<OnlineShopModel> GetAll() =>
            this.unitOfWork.OnlineShopRepository.All().Result;

        private bool IsShopExisting(string newShopUrl) =>
            this.unitOfWork.OnlineShopRepository.All(shop => shop.URL == newShopUrl).Result.Any();

        public bool IsShopExisting(int shopId) =>
            this.unitOfWork.OnlineShopRepository.All(shop => shop.Id == shopId).Result.Any();
    }
}
