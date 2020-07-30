﻿using ItemPriceCharts.Services.Services;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class PhoneShopViewModel : ShopViewModel
    {
        private readonly WebPageService webPageService;

        public PhoneShopViewModel(WebPageService webPageService)
            : base (webPageService)
        {
            this.webPageService = webPageService;
        }
    }
}
