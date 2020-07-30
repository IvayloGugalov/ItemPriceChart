using System;
using System.Collections.Generic;
using System.Text;

namespace ItemPriceCharts.Services.Constants
{
    public class KeyWordConstants
    {
        public readonly string PRICE_CONTENT = @"itemprop=""price"" content=""";
        public readonly string ITEM_CONTENT = @"itemprop=""name"" content=""";

        public List<string> GetVarioKeyWord()
        {
            return new List<string>()
            {
                this.PRICE_CONTENT,
                this.ITEM_CONTENT
            };
        }
    }
}
