using System;
using System.Windows;
using System.Windows.Threading;

using Autofac;
using Autofac.Diagnostics;
using Microsoft.EntityFrameworkCore;
using NLog;
using ItemPriceCharts.Infrastructure.Data;
using ItemPriceCharts.UI.WPF.Factories;
using ItemPriceCharts.UI.WPF.Modules;
using ItemPriceCharts.UI.WPF.Services;
using ItemPriceCharts.UI.WPF.ViewModels;
using ItemPriceCharts.UI.WPF.ViewModels.LoginAndRegistration;
using ItemPriceCharts.UI.WPF.Views;
using ItemPriceCharts.UI.WPF.Views.UserControls;

namespace ItemPriceCharts.UI.WPF.Bootstrapper
{
    public class Bootstrapper
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(Bootstrapper));

        private readonly App app;
        private readonly Dispatcher dispatcher;

        private IContainer container;
        private ILifetimeScope lifetimeScope;
        private IViewFactory viewFactory;

        public Bootstrapper(App app, Dispatcher dispatcher)
        {
            this.app = app;
            this.dispatcher = dispatcher;

            var builder = new ContainerBuilder();

            this.ConfigureContainer(builder);
            this.Start(builder);
        }

        public void Stop()
        {
            this.lifetimeScope.Dispose();
            this.container.Dispose();
        }

        private void Start(ContainerBuilder builder)
        {
            this.container = builder.Build();

#if DEBUG
            var tracer = new DefaultDiagnosticTracer();
            tracer.OperationCompleted += (_, args) =>
            {
                Logger.Info(args.TraceContent);
            };

            // Subscribe to the diagnostics with your tracer.
            this.container.SubscribeToDiagnostics(tracer);
#endif

            //LifetimeScope must be init, after all dependencies are registered.
            this.lifetimeScope = this.container.BeginLifetimeScope();
            this.viewFactory = this.lifetimeScope.Resolve<IViewFactory>(new TypedParameter(typeof(ILifetimeScope), this.lifetimeScope));

            this.MigrateDatabase();
            this.BindViewsToViewModels();

            var config = new ConfigureStartUpWindowService(this.app, this.container, this.viewFactory);
            config.ShowStartUpWindow();
        }

        /// <summary>
        /// Register all modules with services.
        /// Do this before trying to Resolve any service from the context.
        /// </summary>
        /// <param name="builder"></param>
        private void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<AutofacModule>();
            builder.RegisterModule(new MappedTypeModules(this.dispatcher));
            builder.RegisterModule<SelfTypeModule>();
            builder.RegisterModule<DatabaseModule>();
            builder.RegisterModule<ViewsAndViewModelsModule>();
        }

        private void MigrateDatabase()
        {
            var dbContext = this.container.Resolve<ModelsContext>();
            dbContext.Database.Migrate();

            if (!dbContext.Database.CanConnect())
            {
                throw new Exception("Can't connect to database.");
            }
        }

        private void BindViewsToViewModels()
        {
            this.viewFactory.RegisterUserControl<LoginViewModel, LoginView>();
            this.viewFactory.RegisterUserControl<RegisterViewModel, RegisterView>();

            this.viewFactory.Register<MainWindowViewModel, MainWindow>();

            this.viewFactory.RegisterUserControl<ItemListingViewModel, ItemListingView>();
            this.viewFactory.RegisterUserControl<ShopsAndItemListingsViewModel, ShopsAndItemListingsView>();

            this.viewFactory.Register<CreateShopViewModel, CreateShopView>();

            this.viewFactory.Register<CreateItemViewModel, CreateItemView>();
            this.viewFactory.Register<DeleteItemViewModel, DeleteItemView>();
            this.viewFactory.Register<ItemInformationViewModel, ItemInformationView>();
        }
    }
}