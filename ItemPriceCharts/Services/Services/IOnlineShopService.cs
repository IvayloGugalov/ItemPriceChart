﻿using System.Collections.Generic;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Services
{
    public interface IOnlineShopService
    {
        OnlineShopModel GetById(int id);
        IEnumerable<OnlineShopModel> GetAll();
        void CreateShop(string shopURL, string shopTitle);
        void UpdateShop(OnlineShopModel onlineShop);
        void DeleteShop(OnlineShopModel onlineShop);
        bool IsShopExisting(int shopId);
    }
}
