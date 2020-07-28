using System;
using System.Collections.Generic;
using System.Windows;
using Services.Data;
using Services.Models;
using Services.Services;
using Services.Strategies;
using UI.WPF.Bootstrapper;

namespace ItemPriceCharts
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

            var bootstrapper = new Bootstrapper(this);
            bootstrapper.Run(mappedTypes);
        }

    }
}
