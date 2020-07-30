using System;
using System.Collections.Generic;
using System.Windows;


using ItemPriceCharts.Services.Data;
using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.Services.Strategies;

namespace ItemPriceCharts.UI.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        readonly Dictionary<Type, Type> mappedTypes = new Dictionary<Type, Type>();

        protected override void OnStartup(StartupEventArgs e)
        {
            this.mappedTypes.Add(typeof(IWebPageService), typeof(WebPageService));
            this.mappedTypes.Add(typeof(IItemService), typeof(ItemService));
            this.mappedTypes.Add(typeof(IOnlineShopService), typeof(OnlineShopService));
            this.mappedTypes.Add(typeof(ModelsContext), typeof(ModelsContext));
            this.mappedTypes.Add(typeof(UnitOfWork), typeof(UnitOfWork));

            var bootstrapper = new Bootstrapper.Bootstrapper(this);
            bootstrapper.Run(mappedTypes);
        }
    }
}
