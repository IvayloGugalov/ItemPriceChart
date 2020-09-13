using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Data
{
    public interface IUnitOfWork
    {
        IRepository<ItemModel> ItemRepository { get; }
        IRepository<OnlineShopModel> OnlineShopRepository { get; }
        IRepository<ItemPrice> ItemPriceRepository { get; }

        void SaveChanges();
    }
}