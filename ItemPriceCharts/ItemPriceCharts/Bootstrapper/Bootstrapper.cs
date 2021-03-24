﻿using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;
using Autofac.Diagnostics;
using Autofac.Core;
using Microsoft.EntityFrameworkCore;
using NLog;

using ItemPriceCharts.UI.WPF.Factories;
using ItemPriceCharts.UI.WPF.Modules;
using ItemPriceCharts.UI.WPF.ViewModels;
using ItemPriceCharts.UI.WPF.Views;
using ItemPriceCharts.UI.WPF.Views.UserControls;

using ItemPriceCharts.XmReaderWriter;
using ItemPriceCharts.XmReaderWriter.XmlActions;
using ItemPriceCharts.XmReaderWriter.User;

namespace ItemPriceCharts.UI.WPF.Bootstrapper
{
    public class Bootstrapper
    {
        private static readonly Logger logger = LogManager.GetLogger(nameof(Bootstrapper));

        private readonly App app;
        private readonly Dictionary<Type, Type> mappedTypes;

        private IContainer container;
        private ILifetimeScope lifetimeScope;
        private IViewFactory viewFactory;

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
            this.container.Dispose();
        }

        private void Start(ContainerBuilder builder)
        {
            this.container = builder.Build();

#if DEBUG
            var tracer = new DefaultDiagnosticTracer();
            tracer.OperationCompleted += (sender, args) =>
            {
                logger.Info(args.TraceContent);
            };

            // Subscribe to the diagnostics with your tracer.
            this.container.SubscribeToDiagnostics(tracer);
#endif

            //LifetimeScope must be init, after all dependencies are registered.
            this.lifetimeScope = this.container.BeginLifetimeScope();
            this.viewFactory = this.lifetimeScope.Resolve<IViewFactory>(new Parameter[] { new NamedParameter("lifetimeScope", this.lifetimeScope) });
            _ = new DialogCreatorFactory(this.viewFactory);

            this.MigrateDatabase();
            this.RegisterViews();
        }

        public void ShowStartUpWindow()
        {
            try
            {
                var (userName, email) = (string.Empty, string.Empty);

                if (XmlCreateFile.EnsureXmlFileExists())
                {
                    if (UserCredentialsSettings.ShouldEnableAutoLogin(out _))
                    {
                        this.ConfigureMainWindow();

                        return;
                    }
                    else
                    {
                        (userName, email) = UserCredentialsSettings.UsernameAndEmail;
                        logger.Debug($"Found credentials\tUsername:{userName}\tEmal{email}.");
                    }
                }

                var parameters = new Parameter[]
                {
                    new NamedParameter(nameof(userName), userName),
                    new NamedParameter(nameof(email), email),
                };

                var loginWindow = this.viewFactory.Resolve<ViewModels.LoginAndRegistration.LoginViewModel>(parameters);

                loginWindow.ShowDialog();

                this.ConfigureMainWindow();
            }
            catch (Exception e)
            {
                logger.Error(e, "Could not show window");
            }
        }

        private void ConfigureMainWindow()
        {
            var mainWindow = this.viewFactory.Resolve<MainWindowViewModel>(System.Array.Empty<Parameter>());
            this.app.MainWindow = mainWindow;
            this.app.MainWindow.Show();
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

            this.viewFactory.RegisterUserControl<ViewModels.LoginAndRegistration.LoginViewModel, LoginView>();
            this.viewFactory.RegisterUserControl<ViewModels.LoginAndRegistration.RegisterViewModel, RegisterView>();

            this.viewFactory.Register<CreateShopViewModel, CreateShopView>();

            this.viewFactory.Register<CreateItemViewModel, CreateItemView>();
            this.viewFactory.Register<DeleteItemViewModel, DeleteItemView>();
            this.viewFactory.Register<ItemInformationViewModel, ItemInformationView>();

            this.viewFactory.Register<ViewModels.LoginAndRegistration.LoginViewModel, LoginRegisterView>();
        }
    }
}