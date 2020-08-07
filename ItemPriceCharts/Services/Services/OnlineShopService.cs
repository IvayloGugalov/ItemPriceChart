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

        public OnlineShopModel GetById(int id)
        {
            return this.unitOfWork.OnlineShopRepository.All()
                .FirstOrDefault(shop => shop.Id == id) ?? throw new Exception();
        }

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

        public void Update(OnlineShopModel onlineShop)
        {
            try
            {
                if (this.IsShopExisting(onlineShop.Id))
                {
                    var shopToUpdate = this.GetById(onlineShop.Id);
                    this.unitOfWork.OnlineShopRepository.Update(shopToUpdate);
                    this.unitOfWork.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void Delete(int id)
        {
            try
            {
                if (this.IsShopExisting(id))
                {
                    var shopToDelete = this.GetById(id);
                    this.unitOfWork.OnlineShopRepository.Delete(shopToDelete.Id);
                    this.unitOfWork.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<OnlineShopModel> GetAll()
        {
            return this.unitOfWork.OnlineShopRepository.All();
        }

        private bool IsShopExisting(string newShopUrl)
        {
            return this.unitOfWork.OnlineShopRepository.All()
                .Any(item => item.URL == newShopUrl);
        }

        public bool IsShopExisting(int shopId)
        {
            return this.unitOfWork.OnlineShopRepository.All()
                .Any(item => item.Id == shopId);
        }
    }
}
