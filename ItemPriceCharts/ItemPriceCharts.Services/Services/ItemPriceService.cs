using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using NLog;

using ItemPriceCharts.Services.Data;
using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Services
{
    public class ItemPriceService : IItemPriceService
    {
        private static readonly Logger logger = LogManager.GetLogger(nameof(ItemPriceService));

        private readonly IRepository<ItemPrice> itemPriceRepository;
        private readonly IRepository<Item> itemRepository;

        public ItemPriceService(IRepository<ItemPrice> itemPriceRepository, IRepository<Item> itemRepository)
        {
            this.itemPriceRepository = itemPriceRepository;
            this.itemRepository = itemRepository;
        }

        public Task<IEnumerable<ItemPrice>> GetPricesForItem(int itemId) =>
           this.itemPriceRepository.GetAll(filter: i => i.ItemId == itemId);

        public async Task<ItemPrice> CreateItemPrice(double price, int itemId)
        {
            try
            {
                if (this.IsItemExisting(itemId))
                {
                    var itemPrice = new ItemPrice(price, itemId);

                    var createdItemPrice = await this.itemPriceRepository.Add(itemPrice).ConfigureAwait(false);

                    logger.Debug($"Saved '{itemPrice}' for itemId: {itemId} in database.");

                    return createdItemPrice;
                }
                return null;
            }
            catch (Exception e)
            {
                logger.Error(e, $"Error when saving item price for itemId: {itemId}.");
                throw;
            }
        }

        public async Task<double> GetLatestItemPrice(int itemId)
        {
            try
            {
                var pricesForItem = await this.itemPriceRepository.GetAll(
                    filter: price => price.ItemId == itemId,
                    orderBy: prices => prices.OrderByDescending(price => price.PriceDate));

                return pricesForItem.Any() ? pricesForItem.First().Price : 0;
            }
            catch (Exception e)
            {
                logger.Error(e, $"Error when retrieving item price for itemId: {itemId}.");
                throw;
            }
        }

        private bool IsItemExisting(int id) =>
           this.itemRepository.IsExisting(id).GetAwaiter().GetResult();
    }
}
