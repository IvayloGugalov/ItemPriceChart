using System;
using System.Collections.Generic;
using System.Windows;

using Microsoft.EntityFrameworkCore;
using NLog;

using ItemPriceCharts.Services.Data;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.Helpers;

namespace ItemPriceCharts.UI.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly Dictionary<Type, Type> mappedTypes = new Dictionary<Type, Type>();
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        protected override void OnStartup(StartupEventArgs e)
        {
#if DEBUG
            LogHelper.ReconfigureLoggerToLevel(LogLevel.Debug);
#endif
            logger.Info("Starting Application");
            logger.Debug($"Dispatcher managed thread identifier = {System.Threading.Thread.CurrentThread.ManagedThreadId}");

            this.mappedTypes.Add(typeof(IItemService), typeof(ItemService));
            this.mappedTypes.Add(typeof(IOnlineShopService), typeof(OnlineShopService));
            this.mappedTypes.Add(typeof(IItemPriceService), typeof(ItemPriceService));
            this.mappedTypes.Add(typeof(DbContext), typeof(ModelsContext));

            var bootstrapper = new Bootstrapper.Bootstrapper(this);
            bootstrapper.Run(this.mappedTypes);

            MainWindow.Closed += (s, a) =>
            {
                bootstrapper.Stop();
            };

            Application.Current.Exit += (s, e) =>
            {
                logger.Info("Exiting application");
                NLog.LogManager.Shutdown();
            };
        }
    }
}
