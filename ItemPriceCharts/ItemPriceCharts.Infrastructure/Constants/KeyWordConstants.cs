using System;
using System.IO;

namespace ItemPriceCharts.InfraStructure.Constants
{
    public static class PlesioKeyWordConstants
    {
        public const string TITLE = "//h1[@class='productTitle']";
        public const string PRICE = "//span[@class='productPrice']";
        public const string DESCRIPTION_LABELS = "//li//span[@class='characteristicLabel']";
        public const string DESCRIPTION_VALUES = "//li//span[@class='characteristicValue']";
    }

    public static class VarioKeyWordConstants
    {
        public const string TITLE = "//h1[@itemprop]";
        public const string PRICE = "//span[@itemprop]";
        public const string DESCRIPTION_LABELS = "//ul[@class='components_list']//li//h3";
        public const string DESCRIPTION_VALUES = "//ul[@class='components_list']//li//div[@class='supplement_details']";
    }

    public static class DatabaseKeyWordConstants
    {
        public const string DATABASE_LOG_PATH = @"\ItemPriceCharts\database.log";
        public const string CONNECTION_STRING = "Data Source=ItemPriceChartsDB.db";
    }

    public static class Paths
    {
        public static readonly string APPLICATION_APPDATA_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ItemPriceCharts");

        public static readonly string XML_FILE_PATH = Path.Combine(APPLICATION_APPDATA_PATH, "myXmlFile.xml");
    }
}
