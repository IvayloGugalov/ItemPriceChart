using System;
using System.Collections.Generic;
using System.Linq;
using Services.Data;
using Services.Models;

namespace Services.Strategies
{
    public class OnlineShopService : IOnlineShopService
    {
        private readonly UnitOfWork unitOfWork;

        public OnlineShopService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public OnlineShopModel GetById(int id)
        {
            return this.unitOfWork.OnlineShopRepository.All()
                .FirstOrDefault(shop => shop.ShopId == id) ?? throw new Exception();
        }

        public void Add(OnlineShopModel onlineShop)
        {
            try
            {
                if (!this.IsShopExisting(onlineShop.Url))
                {
                    this.unitOfWork.OnlineShopRepository.Add(onlineShop);
                    this.unitOfWork.SaveChanges();
                }
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
                if (this.IsShopExisting(onlineShop.ShopId))
                {
                    var shopToUpdate = this.GetById(onlineShop.ShopId);
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
                    this.unitOfWork.OnlineShopRepository.Delete(shopToDelete.ShopId);
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
                .Any(item => item.Url == newShopUrl);
        }

        private bool IsShopExisting(int shopId)
        {
            return this.unitOfWork.OnlineShopRepository.All()
                .Any(item => item.ShopId == shopId);
        }
    }
}
