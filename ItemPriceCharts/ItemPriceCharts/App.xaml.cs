using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

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
        private static readonly Logger logger = LogManager.GetLogger(nameof(App));

        public App()
        {
#if DEBUG
            LogHelper.ReconfigureLoggerToLevel(LogLevel.Debug);
#endif

            AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
            Application.Current.DispatcherUnhandledException += DispatcherOnUnhandledException;
            TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            logger.Info("Starting Application");
            logger.Debug($"Dispatcher managed thread identifier = {System.Threading.Thread.CurrentThread.ManagedThreadId}");

            this.mappedTypes.Add(typeof(IItemService), typeof(ItemService));
            this.mappedTypes.Add(typeof(IOnlineShopService), typeof(OnlineShopService));
            this.mappedTypes.Add(typeof(IItemPriceService), typeof(ItemPriceService));
            this.mappedTypes.Add(typeof(DbContext), typeof(ModelsContext));

            var bootstrapper = new Bootstrapper.Bootstrapper(this, this.mappedTypes);

            MainWindow.Closed += (s, a) =>
            {
                bootstrapper.Stop();
            };

            Application.Current.Exit += (s, e) =>
            {
                logger.Info("Exiting application");
                NLog.LogManager.Shutdown();
            };

            logger.Info("Application started");
        }

        private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs args)
        {
            args.SetObserved();
            this.HandleException(args.Exception.GetBaseException());
        }

        private void DispatcherOnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args)
        {
            args.Handled = true;
            this.HandleException(args.Exception);
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs args)
        {
            this.HandleException(args.ExceptionObject as Exception);
        }

        private void HandleException(Exception exception)
        {
            logger.Error(exception);

            UIEvents.ShowMessageDialog(
                new ViewModels.MessageDialogViewModel(
                    title: nameof(exception),
                    description: exception.Message,
                    buttonType: ViewModels.ButtonType.Close));
        }
    }
}
