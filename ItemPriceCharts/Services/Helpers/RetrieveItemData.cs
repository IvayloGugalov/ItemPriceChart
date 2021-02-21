using System;
using System.Collections.Generic;
using System.Linq;

using HtmlAgilityPack;
using NLog;

using ItemPriceCharts.Services.Constants;
using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.Services.Helpers
{
    public static class RetrieveItemData
    {
        private static readonly Logger logger = LogManager.GetLogger(nameof(RetrieveItemData));

        public static Item CreateItem(string itemURL, HtmlDocument itemDocument, OnlineShop onlineShop, ItemType type)
        {
            switch (onlineShop.Title)
            {
                case "Vario":
                {
                    var data = VarioRetrieveData(itemDocument);

                    return new Item(
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

                    return new Item(
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
                double.TryParse(htmlDocument.DocumentNode.SelectSingleNode(PlesioKeyWordConstants.PRICE).InnerText.Trim(toTrim)
                    .Replace("лв", "")
                    .Trim().Replace(".", ","), out var price);

                var descriptioLabels = htmlDocument.DocumentNode.SelectNodes(PlesioKeyWordConstants.DESCRIPTION_LABELS).Select(x => x.InnerText);
                var descriptionValues = htmlDocument.DocumentNode.SelectNodes(PlesioKeyWordConstants.DESCRIPTION_VALUES).Select(x => x.InnerText);
                var description = GetDescription(descriptioLabels, descriptionValues);

                return Tuple.Create(title, description, price);
            }
            catch (Exception e)
            {
                logger.Error($"Can't get information for item: {e}");
                throw;
            }
        }

        private static Tuple<string, string, double> VarioRetrieveData(HtmlDocument htmlDocument)
        {
            try
            {
                var title = htmlDocument.DocumentNode.SelectSingleNode(VarioKeyWordConstants.TITLE).GetAttributeValue("content", "");
                double.TryParse(htmlDocument.DocumentNode.SelectSingleNode(VarioKeyWordConstants.PRICE).GetAttributeValue("content", ""), out var price);

                var descriptioLabels = htmlDocument.DocumentNode.SelectNodes(VarioKeyWordConstants.DESCRIPTION_LABELS).Select(x => x.InnerText);
                var descriptionValues = htmlDocument.DocumentNode.SelectNodes(VarioKeyWordConstants.DESCRIPTION_VALUES).Select(x => x.InnerText);
                var description = GetDescription(descriptioLabels, descriptionValues);

                return Tuple.Create(title, description, price);
            }
            catch (Exception e)
            {
                logger.Error($"Can't get information for item: {e}");
                throw;
            }
        }

        private static string GetDescription(IEnumerable<string> descriptioLabels, IEnumerable<string> descriptionValues)
        {
            var unformatedDescription = descriptioLabels.Zip(descriptionValues, (label, value) => label + "\t" + value);
            return string.Join("\n", unformatedDescription);
        }
    }
}
