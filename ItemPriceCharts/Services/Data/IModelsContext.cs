using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Data
{
    public interface IModelsContext
    {
        IRepository<Item> ItemRepository { get; }
        IRepository<OnlineShop> OnlineShopRepository { get; }
        IRepository<ItemPrice> ItemPriceRepository { get; }

        void CommitChangesAsync();
    }
}