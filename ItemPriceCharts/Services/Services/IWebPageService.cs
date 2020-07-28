using System.Collections.Generic;
using Services.Models;
using Services.Models.Entities;

namespace Services.Services
{
    public interface IWebPageService
    {
        /// <summary>
        /// Create Vario Item
        /// </summary>
        /// <param name="itemURL"></param>
        /// <param name="shopId"></param>
        void CreateItem(string itemURL, int shopId);

        void DeleteShop(int id);

        IEnumerable<OnlineShopModel> RetrieveItemsForShop();

        IEnumerable<OnlineShopModel> RetrieveOnlineShops();

        /// <summary>
        /// Vario Items
        /// </summary>
        /// <returns></returns>
        List<ItemResult> FindRequiredTextForPC();
    }
}