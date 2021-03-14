using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;
using Autofac.Core;
using Microsoft.EntityFrameworkCore;

using ItemPriceCharts.UI.WPF.Factories;
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
            _ = new DialogCreatorFactory(this.viewFactory);

            this.MigrateDatabase();
            this.RegisterViews();
            this.ConfigureApplication();

            var login = this.viewFactory.Resolve<ViewModels.LoginAndRegistration.StartUpViewModel>(System.Array.Empty<Parameter>());
            login.Show();
        }

        private void MigrateDatabase()
        {
            using (Services.Data.ModelsContext dbContext = new Services.Data.ModelsContext())
            {
                dbContext.Database.Migrate();
                if (!dbContext.Database.CanConnect())
                {
                    throw new Exception("Can't connect to database.");
                }
            }
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

            this.viewFactory.Register<CreateItemViewModel, CreateItemView>();
            this.viewFactory.Register<DeleteItemViewModel, DeleteItemView>();
            this.viewFactory.Register<ItemInformationViewModel, ItemInformationView>();

            this.viewFactory.Register<ViewModels.LoginAndRegistration.StartUpViewModel, LoginRegisterView>();
        }

        private void ConfigureApplication()
        {
            var mainWindow = this.viewFactory.Resolve<MainWindowViewModel>(System.Array.Empty<Parameter>());
            this.app.MainWindow = mainWindow;
            this.app.MainWindow.Show();
        }
    }
}