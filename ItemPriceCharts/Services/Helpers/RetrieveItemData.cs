using System;
using System.Linq;

using HtmlAgilityPack;

using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Helpers
{
    public static class RetrieveItemData
    {

        public static ItemModel CreateModel(string itemURL, HtmlDocument itemDocument, OnlineShopModel onlineShop, ItemModel.ItemType type)
        {
            switch (onlineShop.Title)
            {
                case "Vario":
                {
                    var data = VarioRetrieveData(itemDocument);

                    return new ItemModel(
                        id: default,
                        url: itemURL,
                        title: data.Item1,
                        description: data.Item2,
                        price: data.Item3,
                        onlineShop: onlineShop,
                        type: type);
                }
                case "Plesio":
                {
                    var data = PlesioRetrieveData(itemDocument);

                    return new ItemModel(
                        id: default,
                        url: itemURL,
                        title: data.Item1,
                        description: data.Item2,
                        price: data.Item3,
                        onlineShop: onlineShop,
                        type: type);
                }
                default:
                    return null;
            }
        }

        private static Tuple<string, string, double> PlesioRetrieveData (HtmlDocument htmlDocument)
        {
            char[] toTrim = { '\n', ' ' };
            try
            {
                var title = htmlDocument.DocumentNode.SelectSingleNode("//h1[@class='productTitle']").InnerText;
                var description = string.Empty;
                double.TryParse(htmlDocument.DocumentNode.SelectSingleNode("//span[@class='productPrice']").InnerText.Trim(toTrim)
                    .Replace("лв", "")
                    .Trim().Replace(".", ","), out var price);

                var descriptionValues = htmlDocument.DocumentNode.SelectNodes("//li//span[@class='characteristicLabel']")
                    .Select(x => x.InnerText);
                var descriptioLabels = htmlDocument.DocumentNode.SelectNodes("//li//span[@class='characteristicValue']")
                    .Select(x => x.InnerText);

                foreach (var item in descriptionValues.Zip(descriptioLabels, (a, b) => a + "\t" + b + "\n"))
                {
                    description += item;
                }

                return Tuple.Create(title, description, price);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        private static Tuple<string, string, double> VarioRetrieveData(HtmlDocument htmlDocument)
        {
            try
            {
                var title = htmlDocument.DocumentNode.SelectSingleNode("//h1[@itemprop]").GetAttributeValue("content", "");
                var description = string.Empty;
                double.TryParse(htmlDocument.DocumentNode.SelectSingleNode("//span[@itemprop]").GetAttributeValue("content", ""), out var price);

                var descriptionValues = htmlDocument.DocumentNode.SelectNodes("//li//span").Select(x => x.InnerText);
                var descriptioLabels = htmlDocument.DocumentNode.SelectNodes("//div//ul//li//h3").Select(x => x.InnerText);

                foreach (var item in descriptioLabels.Zip(descriptionValues, (a, b) => a + "\t" + b + "\n"))
                {
                    description += item;
                }

                return Tuple.Create(title, description, price);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
