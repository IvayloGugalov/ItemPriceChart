using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using NLog;

using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.Infrastructure.Services;

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
            this.mappedTypes.Add(typeof(IUserAccountService), typeof(UserAccountService));

            var bootstrapper = new Bootstrapper.Bootstrapper(this, this.mappedTypes);

            Application.Current.Exit += (s, e) =>
            {
                logger.Info("Exiting application");
                bootstrapper.Stop();
                NLog.LogManager.Shutdown();
            };

            bootstrapper.ShowStartUpWindow();

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

            if (UIEvents.ShowMessageDialog is not null)
            {
                UIEvents.ShowMessageDialog(
                    new ViewModels.MessageDialogViewModel(
                        title: nameof(exception),
                        description: exception.Message,
                        buttonType: ViewModels.ButtonType.Close));
            }
            else
            {
                MessageBox.Show(
                    messageBoxText: exception.Message,
                    caption: nameof(exception),
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }

            Application.Current.Shutdown();
        }
    }
}
