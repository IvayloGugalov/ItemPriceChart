using System;
using System.Linq;

using HtmlAgilityPack;
using ItemPriceCharts.Services.Constants;
using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Helpers
{
    public static class RetrieveItemData
    {

        public static ItemModel CreateModel(string itemURL, HtmlDocument itemDocument, OnlineShopModel onlineShop, ItemType type)
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
                var title = htmlDocument.DocumentNode.SelectSingleNode(PlesioKeyWordConstants.TITLE).InnerText;
                var description = string.Empty;
                double.TryParse(htmlDocument.DocumentNode.SelectSingleNode(PlesioKeyWordConstants.PRICE).InnerText.Trim(toTrim)
                    .Replace("лв", "")
                    .Trim().Replace(".", ","), out var price);

                var descriptionValues = htmlDocument.DocumentNode.SelectNodes(PlesioKeyWordConstants.DESCRIPTION_VALUES)
                    .Select(x => x.InnerText);
                var descriptioLabels = htmlDocument.DocumentNode.SelectNodes(PlesioKeyWordConstants.DESCRIPTION_LABELS)
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
                var title = htmlDocument.DocumentNode.SelectSingleNode(VarioKeyWordConstants.TITLE).GetAttributeValue("content", "");
                var description = string.Empty;
                double.TryParse(htmlDocument.DocumentNode.SelectSingleNode(VarioKeyWordConstants.PRICE).GetAttributeValue("content", ""), out var price);

                var descriptionValues = htmlDocument.DocumentNode.SelectNodes(VarioKeyWordConstants.DESCRIPTION_VALUES).Select(x => x.InnerText);
                var descriptioLabels = htmlDocument.DocumentNode.SelectNodes(VarioKeyWordConstants.DESCRIPTION_LABELS).Select(x => x.InnerText);

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
