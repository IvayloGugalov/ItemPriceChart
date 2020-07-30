using System;
using System.Collections.Generic;
using System.Text;

namespace ItemPriceCharts.Services.Constants
{
    public class URLConstants
    {
        private readonly string MSI470Vario = "https://www.vario.bg/msi-x470-gaming-pro-max-7b79-007r";
        private readonly string GSKILL3600Vario = "https://www.vario.bg/gskill-trident-z-rgb-dimm-kit-16gb-f4-3600c16d-16gtzrc";
        private readonly string ADATASX8200 = "https://www.vario.bg/adata-xpg-sx8200-pro-512gb-asx8200pnp-512gt-c";

        public List<string> GetVarioURL()
        {
            return new List<string>() 
            {
                this.MSI470Vario,
                this.GSKILL3600Vario,
                this.ADATASX8200
            };
        }
    }
}
