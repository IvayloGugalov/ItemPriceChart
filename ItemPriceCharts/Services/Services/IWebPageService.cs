using System.Collections.Generic;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Services
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
        List<ItemModel> FindRequiredTextForPC();
    }
}