using System;
using System.Collections.Generic;
using System.Windows;

using Microsoft.EntityFrameworkCore;

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
        private readonly Dictionary<Type, Type> mappedTypes = new Dictionary<Type, Type>();

        protected override void OnStartup(StartupEventArgs e)
        {
            this.mappedTypes.Add(typeof(IWebPageService), typeof(WebPageService));
            this.mappedTypes.Add(typeof(IItemService), typeof(ItemService));
            this.mappedTypes.Add(typeof(IOnlineShopService), typeof(OnlineShopService));
            this.mappedTypes.Add(typeof(IUnitOfWork), typeof(UnitOfWork));
            this.mappedTypes.Add(typeof(DbContext), typeof(ModelsContext));

            var bootstrapper = new Bootstrapper.Bootstrapper(this);
            bootstrapper.Run(this.mappedTypes);

            this.MainWindow.Closed += (s, a) =>
            {
                bootstrapper.Stop();
            };
        }
    }
}
