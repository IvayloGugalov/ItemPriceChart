using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using Autofac;
using Autofac.Core;

using ItemPriceCharts.Services.Models;
using ItemPriceCharts.UI.WPF.Factories;
using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.UI.WPF.Modules;
using ItemPriceCharts.UI.WPF.ViewModels;
using ItemPriceCharts.UI.WPF.Views;
using ItemPriceCharts.UI.WPF.Views.UserControls;

namespace ItemPriceCharts.UI.WPF.Bootstrapper
{
    public class Bootstrapper
    {
        private readonly App app;
        private readonly Dictionary<Type, Type> mappedTypes;
        private IViewFactory viewFactory;
        private ILifetimeScope lifetimeScope;

        public Bootstrapper(App app, Dictionary<Type, Type> mappedTypes)
        {
            this.app = app;
            this.mappedTypes = mappedTypes;

            var builder = new ContainerBuilder();

            this.ConfigureContainer(builder);
            this.Start(builder);

            _ = new DialogCreatorFactory(this.viewFactory);
        }

        public void Stop()
        {
            this.lifetimeScope.Dispose();
        }

        private void Start(ContainerBuilder builder)
        {
            //LifetimeScope must be init, after all dependencies are registered.
            this.lifetimeScope = builder.Build();
            this.viewFactory = this.lifetimeScope.Resolve<IViewFactory>(new Parameter[] { new NamedParameter("lifetimeScope", this.lifetimeScope) });

            this.RegisterViews();
            this.ConfigureApplication();
        }

        private void ConfigureContainer(ContainerBuilder builder)
        {
            if (this.mappedTypes != null && this.mappedTypes.Any())
            {
                builder.RegisterModule(new MappedTypeModules(this.mappedTypes));
            }

            builder.RegisterModule<MainModule>();
            builder.RegisterModule<ViewModelsModule>();
            builder.RegisterModule<AutofacModule>();
        }

        private void RegisterViews()
        {
            this.viewFactory.Register<MainWindowViewModel, MainWindow>();

            this.viewFactory.RegisterUserControl<ItemListingViewModel, ItemListingView>();
            this.viewFactory.RegisterUserControl<ShopsAndItemListingsViewModel, ShopsAndItemListingsView>();

            this.viewFactory.Register<CreateShopViewModel, CreateShopView>();
            this.viewFactory.Register<DeleteShopViewModel, DeleteShopView>();

            this.viewFactory.Register<CreateItemViewModel, CreateItemView>();
            this.viewFactory.Register<DeleteItemViewModel, DeleteItemView>();
            this.viewFactory.Register<ItemInformationViewModel, ItemInformationView>();
        }

        private void ConfigureApplication()
        {
            var mainWindow = this.viewFactory.Resolve<MainWindowViewModel>(System.Array.Empty<Parameter>());
            this.app.MainWindow = mainWindow;
            this.app.MainWindow.Show();
        }
    }
}