﻿using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Threading;

using NLog;

using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.UI.WPF.Events;
using ItemPriceCharts.UI.WPF.Factories;
using ItemPriceCharts.UI.WPF.Services;

namespace ItemPriceCharts.UI.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
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

            // Set the current Culture to the app
            FrameworkElement.LanguageProperty.OverrideMetadata(
                typeof(FrameworkElement),
                new FrameworkPropertyMetadata(
                    XmlLanguage.GetLanguage(
                        CultureInfo.CurrentCulture.IetfLanguageTag)));

            Bootstrapper.Bootstrapper.Start(new DispatcherWrapper(this.Dispatcher));

            Application.Current.Exit += (_, _) =>
            {
                Logger.Info("Exiting application");
                Bootstrapper.Bootstrapper.Stop();
                LogManager.Shutdown();
            };

            Logger.Info("Application started");

            var config = new StartUpService(this);
            config.ShowStartUpWindow();
            config.Dispose();

            Logger.Info("Finished start up window navigation");
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
