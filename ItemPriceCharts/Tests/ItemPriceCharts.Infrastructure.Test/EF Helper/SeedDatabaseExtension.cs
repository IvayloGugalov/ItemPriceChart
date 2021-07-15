using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.Domain.Enums;
using ItemPriceCharts.Infrastructure.Data;

namespace ItemPriceCharts.Infrastructure.Test.EF_Helper
{
    public static class SeedDatabaseExtension
    {
        public static async Task SeedDatabaseWithShops(this ModelsContext dbContext, int range)
        {
            var shops = new List<OnlineShop>();
            for (int i = 0; i < range; i++)
            {
                var title = new Random(10000).Next().ToString();
                var url = $"https://www.{title}.com";
                shops.Add(new OnlineShop(url: url, title: title));
            }

            await dbContext.OnlineShops.AddRangeAsync(shops);
            await dbContext.SaveChangesAsync();
        }

        public static async Task SeedDatabaseWithItems(this ModelsContext dbContext, int range, OnlineShop onlineShop = null)
        {
            onlineShop ??= new OnlineShop(title: "online shop title", url: "https://www.shopUrl.com");

            dbContext.OnlineShops.Add(onlineShop);
            dbContext.SaveChanges();

            for (int i = 0; i < range; i++)
            {
                var title = new Random(10000).Next() + i;
                var description = $"description for item {title}";

                onlineShop.AddItem(Item.Construct(
                    id: Guid.NewGuid(),
                    url: $"{onlineShop.Url}//{title}",
                    title: title.ToString(),
                    description: description,
                    price: new Random(10000).NextDouble(),
                    onlineShop: onlineShop,
                    type: ItemType.ComputerItem));
            }

            await dbContext.SaveChangesAsync();
        }
    }
}
