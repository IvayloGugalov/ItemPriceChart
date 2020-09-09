using ItemPriceCharts.Services.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class ItemInformationViewModel : BindableViewModel
    {
        public ItemModel Item { get; }


        public ItemInformationViewModel(ItemModel item)
        {
            this.Item = item;
        }
    }
}
