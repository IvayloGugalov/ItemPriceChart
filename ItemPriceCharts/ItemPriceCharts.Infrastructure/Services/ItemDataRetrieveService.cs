using System;
using System.Collections.Generic;
using System.Linq;

using HtmlAgilityPack;
using NLog;

using ItemPriceCharts.InfraStructure.Constants;

namespace ItemPriceCharts.Infrastructure.Services
{
    public static class ItemDataRetrieveService
    {
        private static readonly Logger logger = LogManager.GetLogger(nameof(ItemDataRetrieveService));

        // Change Title to a more concrete parameter: id, const....
        public static (string title, string description, double price) CreateItem(HtmlDocument itemDocument, string onlineShopTitle)
        {
            switch (onlineShopTitle)
            {
                case "Vario":
                    {
                        return ItemDataRetrieveService.VarioRetrieveData(itemDocument);
                    }
                case "Plesio":
                    {
                        return ItemDataRetrieveService.PlesioRetrieveData(itemDocument);
                    }
                default:
                    throw new NonExistenShopException($"Shop with title {onlineShopTitle} is non existent.");
            }
        }

        private static (string title, string description, double price) PlesioRetrieveData(HtmlDocument htmlDocument)
        {
            char[] toTrim = { '\n', ' ' };
            try
            {
                var title = htmlDocument.DocumentNode.SelectSingleNode(PlesioKeyWordConstants.TITLE).InnerText;
                _ = double.TryParse(htmlDocument.DocumentNode.SelectSingleNode(PlesioKeyWordConstants.PRICE).InnerText.Trim(toTrim)
                    .Replace("лв", "")
                    .Trim().Replace(".", ","), out var price);

                var descriptioLabels = htmlDocument.DocumentNode.SelectNodes(PlesioKeyWordConstants.DESCRIPTION_LABELS).Select(x => x.InnerText);
                var descriptionValues = htmlDocument.DocumentNode.SelectNodes(PlesioKeyWordConstants.DESCRIPTION_VALUES).Select(x => x.InnerText);
                var description = ItemDataRetrieveService.GetDescription(descriptioLabels, descriptionValues);

                return (title, description, price);
            }
            catch (Exception e)
            {
                logger.Error($"Can't get information for item: {e}");
                throw;
            }
        }

        private static (string title, string description, double price) VarioRetrieveData(HtmlDocument htmlDocument)
        {
            try
            {
                var title = htmlDocument.DocumentNode.SelectSingleNode(VarioKeyWordConstants.TITLE).GetAttributeValue("content", "");
                _ = double.TryParse(htmlDocument.DocumentNode.SelectSingleNode(VarioKeyWordConstants.PRICE).GetAttributeValue("content", ""), out var price);

                var descriptioLabels = htmlDocument.DocumentNode.SelectNodes(VarioKeyWordConstants.DESCRIPTION_LABELS).Select(x => x.InnerText);
                var descriptionValues = htmlDocument.DocumentNode.SelectNodes(VarioKeyWordConstants.DESCRIPTION_VALUES).Select(x => x.InnerText);
                var description = ItemDataRetrieveService.GetDescription(descriptioLabels, descriptionValues);

                return (title, description, price);
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

        private class NonExistenShopException : Exception
        {
            public NonExistenShopException(string message) : base(message)
            {
            }
        }
    }
}
