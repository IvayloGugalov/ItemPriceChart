using HtmlAgilityPack;

namespace ItemPriceCharts.Infrastructure.Services
{
    public interface IItemDataRetrieveService
    {
        (string title, string description, double price) CreateItem(HtmlDocument itemDocument, string onlineShopTitle);
    }
}