using System;
using System.Collections.Generic;
using System.Text;

namespace ItemPriceCharts.UI.WPF.Helpers
{
    public class Events
    {
        //public static PublishAndSubscribeEventArgs<string> NewShopAdded { get; set; }

        public Events()
        {
            PublishSubscribe<string>.RegisterEvent("NewShopAdded", this.EventHandler);
        }

        private void EventHandler(object sender, PublishAndSubscribeEventArgs<string> args)
        {

        }
    }
}
