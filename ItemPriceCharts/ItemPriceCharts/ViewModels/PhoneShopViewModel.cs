using Services.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace UI.WPF.ViewModels
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
