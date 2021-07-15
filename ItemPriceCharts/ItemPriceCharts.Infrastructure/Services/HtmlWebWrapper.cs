using HtmlAgilityPack;

namespace ItemPriceCharts.Infrastructure.Services
{
    public class HtmlWebWrapper : IHtmlWebWrapper
    {
        private readonly HtmlWeb htmlWeb;

        public HtmlWebWrapper(HtmlWeb htmlWeb)
        {
            this.htmlWeb = htmlWeb;
        }

        public HtmlDocument LoadDocument(string url)
        {
            return this.htmlWeb.Load(url);
        }
    }
}
