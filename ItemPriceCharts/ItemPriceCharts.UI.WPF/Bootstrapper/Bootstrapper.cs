using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;
using Autofac.Diagnostics;
using Autofac.Core;
using Microsoft.EntityFrameworkCore;

using NLog;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.Infrastructure.Data;
using ItemPriceCharts.Infrastructure.Services;
using ItemPriceCharts.UI.WPF.Factories;
using ItemPriceCharts.UI.WPF.Modules;
using ItemPriceCharts.UI.WPF.ViewModels;
using ItemPriceCharts.UI.WPF.Views;
using ItemPriceCharts.UI.WPF.Views.UserControls;
using ItemPriceCharts.XmReaderWriter.XmlActions;
using ItemPriceCharts.XmReaderWriter.User;

namespace ItemPriceCharts.UI.WPF.Bootstrapper
{
    public class Bootstrapper
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(Bootstrapper));

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
            tracer.OperationCompleted += (_, args) =>
            {
                Logger.Info(args.TraceContent);
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
                var accountService = this.container.Resolve<IUserAccountService>();

                if (XmlCreateFile.EnsureXmlFileExists())
                {
                    UserCredentialsSettings.ReadSettings();

                    if (UserCredentialsSettings.ShouldEnableAutoLogin)
                    {
                        var userAccount = accountService.GetUserAccount(UserCredentialsSettings.Username, UserCredentialsSettings.Email).GetAwaiter().GetResult();

                        this.ConfigureMainWindow(userAccount);

                        return;
                    }

                    (userName, email) = UserCredentialsSettings.UsernameAndEmail;
                    Logger.Debug($"Found credentials\tUsername:{userName}\tEmail:{email}.");
                }

                var loginViewModel = new ViewModels.LoginAndRegistration.LoginViewModel(accountService, userName, email);
                Helpers.UiEvents.ShowLoginRegisterWindow(loginViewModel);

                if (loginViewModel.SuccessfulLogin)
                {
                    this.ConfigureMainWindow(loginViewModel.UserAccount);
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, "Could not show window");
                throw new Exception("We are having difficulties with the app, please send us the logs!");
            }
        }

        private void ConfigureMainWindow(UserAccount userAccount)
        {
            var mainWindow = this.viewFactory.Resolve<MainWindowViewModel>(new Parameter[] { new NamedParameter(nameof(userAccount), userAccount) });
            this.app.MainWindow = mainWindow;
            this.app.MainWindow?.Show();
        }

        private void MigrateDatabase()
        {
            using ModelsContext dbContext = new ModelsContext();
            dbContext.Database.Migrate();
            if (!dbContext.Database.CanConnect())
            {
                throw new Exception("Can't connect to database.");
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
        }
    }
}