using System;
using System.Collections.Generic;
using System.Text;
using Services.Models;

namespace Services.Strategies
{
    public interface IOnlineShopService
    {
        OnlineShopModel GetById(int id);
        IEnumerable<OnlineShopModel> GetAll();
        void Add(OnlineShopModel onlineShop);
        void Update(OnlineShopModel onlineShop);
        void Delete(int id);
    }
}
