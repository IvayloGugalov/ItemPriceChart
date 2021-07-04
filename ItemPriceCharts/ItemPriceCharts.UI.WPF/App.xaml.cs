using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using NLog;

using ItemPriceCharts.Infrastructure.Services;
using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.UI.WPF.Events;

namespace ItemPriceCharts.UI.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly Dictionary<Type, Type> mappedTypes = new Dictionary<Type, Type>();
        private static readonly Logger Logger = LogManager.GetLogger(nameof(App));

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
            Logger.Info("Starting Application");
            Logger.Debug($"Dispatcher managed thread identifier = {System.Threading.Thread.CurrentThread.ManagedThreadId}");

            this.mappedTypes.Add(typeof(IItemService), typeof(ItemService));
            this.mappedTypes.Add(typeof(IOnlineShopService), typeof(OnlineShopService));
            this.mappedTypes.Add(typeof(IUserAccountService), typeof(UserAccountService));
            this.mappedTypes.Add(typeof(IHtmlWebWrapper), typeof(HtmlWebWrapper));
            this.mappedTypes.Add(typeof(IItemDataRetrieveService), typeof(ItemDataRetrieveService));

            var bootstrapper = new Bootstrapper.Bootstrapper(this, this.mappedTypes);

            Application.Current.Exit += (_, _) =>
            {
                Logger.Info("Exiting application");
                bootstrapper.Stop();
                LogManager.Shutdown();
            };

            bootstrapper.ShowStartUpWindow();

            Logger.Info("Application started");
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
            Logger.Error(exception);

            if (UiEvents.ShowMessageDialog is not null)
            {
                UiEvents.ShowMessageDialog(
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
