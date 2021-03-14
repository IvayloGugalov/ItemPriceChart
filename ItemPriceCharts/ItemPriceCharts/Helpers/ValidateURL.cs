using System;

using ItemPriceCharts.Services.Helpers;

namespace ItemPriceCharts.UI.WPF.Helpers
{
    public static class ValidateURL
    {
        public static bool IsValidAddress(string address)
        {
            return Uri.TryCreate(address, UriKind.Absolute, out var uriAddress) &&
                uriAddress.Scheme.IsAny(Uri.UriSchemeHttp, Uri.UriSchemeHttps);
        }
    }
}
