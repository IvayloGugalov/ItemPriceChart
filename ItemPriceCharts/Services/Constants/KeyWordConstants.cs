namespace ItemPriceCharts.Services.Constants
{
    public static class PlesioKeyWordConstants
    {
        public static string TITLE = "//h1[@class='productTitle']";
        public static string PRICE = "//span[@class='productPrice']";
        public static string DESCRIPTION_LABELS = "//li//span[@class='characteristicLabel']";
        public static string DESCRIPTION_VALUES = "//li//span[@class='characteristicValue']";
    }

    public static class VarioKeyWordConstants
    {
        public static string TITLE = "//h1[@itemprop]";
        public static string PRICE = "//span[@itemprop]";
        public static string DESCRIPTION_LABELS = "//ul[@class='components_list']//li//h3";
        public static string DESCRIPTION_VALUES = "//ul[@class='components_list']//li//div[@class='supplement_details']";
    }
}
