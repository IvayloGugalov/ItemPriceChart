using System;
using System.Collections.Generic;
using System.Linq;

using HtmlAgilityPack;
using NLog;

using ItemPriceCharts.InfraStructure.Constants;

namespace ItemPriceCharts.Infrastructure.Services
{
    public class ItemDataRetrieveService : IItemDataRetrieveService
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(ItemDataRetrieveService));

        // Change Title to a more concrete parameter: id, const....
        public (string title, string description, double price) CreateItem(HtmlDocument itemDocument, string onlineShopTitle)
        {
            return onlineShopTitle switch
            {
                "Vario" => ItemDataRetrieveService.VarioRetrieveData(itemDocument),
                "Plesio" => ItemDataRetrieveService.PlesioRetrieveData(itemDocument),
                _ => throw new NonExistentShopException($"Shop with title {onlineShopTitle} is non existent.")
            };
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

                var descriptionLabels = htmlDocument.DocumentNode.SelectNodes(PlesioKeyWordConstants.DESCRIPTION_LABELS).Select(x => x.InnerText);
                var descriptionValues = htmlDocument.DocumentNode.SelectNodes(PlesioKeyWordConstants.DESCRIPTION_VALUES).Select(x => x.InnerText);
                var description = ItemDataRetrieveService.GetDescription(descriptionLabels, descriptionValues);

                return (title, description, price);
            }
            catch (Exception e)
            {
                Logger.Error($"Can't get information for item: {e}");
                throw;
            }
        }

        private static (string title, string description, double price) VarioRetrieveData(HtmlDocument htmlDocument)
        {
            try
            {
                var title = htmlDocument.DocumentNode.SelectSingleNode(VarioKeyWordConstants.TITLE).GetAttributeValue("content", "");
                _ = double.TryParse(htmlDocument.DocumentNode.SelectSingleNode(VarioKeyWordConstants.PRICE).GetAttributeValue("content", ""), out var price);

                var descriptionLabels = htmlDocument.DocumentNode.SelectNodes(VarioKeyWordConstants.DESCRIPTION_LABELS).Select(x => x.InnerText);
                var descriptionValues = htmlDocument.DocumentNode.SelectNodes(VarioKeyWordConstants.DESCRIPTION_VALUES).Select(x => x.InnerText);
                var description = ItemDataRetrieveService.GetDescription(descriptionLabels, descriptionValues);

                return (title, description, price);
            }
            catch (Exception e)
            {
                Logger.Error($"Can't get information for item: {e}");
                throw;
            }
        }

        private static string GetDescription(IEnumerable<string> descriptionLabels, IEnumerable<string> descriptionValues)
        {
            var unformattedDescription = descriptionLabels.Zip(descriptionValues, (label, value) => label + "\t" + value);
            return string.Join("\n", unformattedDescription);
        }

        private class NonExistentShopException : Exception
        {
            public NonExistentShopException(string message) : base(message)
            {
            }
        }
    }
}
