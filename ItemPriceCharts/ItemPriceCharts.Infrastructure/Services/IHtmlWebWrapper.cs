using HtmlAgilityPack;

namespace ItemPriceCharts.Infrastructure.Services
{
    public interface IHtmlWebWrapper
    {
        HtmlDocument LoadDocument(string url);
    }
}
