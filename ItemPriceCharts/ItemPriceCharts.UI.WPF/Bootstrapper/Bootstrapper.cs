using System;
using System.Windows.Threading;

using Autofac;
using Autofac.Core;
using Autofac.Diagnostics;
using Microsoft.EntityFrameworkCore;
using NLog;

using ItemPriceCharts.Infrastructure.Data;
using ItemPriceCharts.UI.WPF.Factories;
using ItemPriceCharts.UI.WPF.Modules;
using ItemPriceCharts.UI.WPF.ViewModels;
using ItemPriceCharts.UI.WPF.ViewModels.LoginAndRegistration;
using ItemPriceCharts.UI.WPF.Views;
using ItemPriceCharts.UI.WPF.Views.UserControls;

namespace ItemPriceCharts.UI.WPF.Bootstrapper
{
    public static class Bootstrapper
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(Bootstrapper));

        private static IContainer container;
        private static ILifetimeScope lifetimeScope;
        private static IDispatcherWrapper dispatcher;

        public static IViewFactory ViewFactory { get; private set; }

        public static void Stop()
        {
            lifetimeScope.Dispose();
            container.Dispose();
        }

        public static void Start(IDispatcherWrapper dispatcher)
        {
            Bootstrapper.dispatcher = dispatcher;

            var builder = new ContainerBuilder();

            ConfigureContainer(builder);

            container = builder.Build();

#if DEBUG
            var tracer = new DefaultDiagnosticTracer();
            tracer.OperationCompleted += (_, args) =>
            {
                Logger.Info(args.TraceContent);
            };

            // Subscribe to the diagnostics with your tracer.
            container.SubscribeToDiagnostics(tracer);
#endif

            //LifetimeScope must be init, after all dependencies are registered.
            lifetimeScope = container.BeginLifetimeScope();
            ViewFactory = lifetimeScope.Resolve<IViewFactory>(new TypedParameter(typeof(ILifetimeScope), lifetimeScope));

            MigrateDatabase();
            BindViewsToViewModels();
            Resolve<DialogCreatorFactory>();
        }

        /// <summary>
        /// Register all modules with services.
        /// Do this before trying to Resolve any service from the context.
        /// </summary>
        /// <param name="builder"></param>
        private static void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<AutofacModule>();
            builder.RegisterModule(new MappedTypeModules(Bootstrapper.dispatcher));
            builder.RegisterModule<SelfTypeModule>();
            builder.RegisterModule<DatabaseModule>();
            builder.RegisterModule<ViewsAndViewModelsModule>();
        }

        private static void MigrateDatabase()
        {
            var dbContext = container.Resolve<ModelsContext>();
            dbContext.Database.Migrate();

            if (!dbContext.Database.CanConnect())
            {
                throw new Exception("Can't connect to database.");
            }

        }

        private static void BindViewsToViewModels()
        {
            ViewFactory.RegisterUserControl<LoginViewModel, LoginView>();
            ViewFactory.RegisterUserControl<RegisterViewModel, RegisterView>();
            ViewFactory.RegisterUserControl<UserSettingsViewModel, UserSettingsModalControl>();

            ViewFactory.Register<MainWindowViewModel, MainWindow>();

            ViewFactory.Register<CreateShopViewModel, CreateShopView>();
            ViewFactory.Register<CreateItemViewModel, CreateItemView>();
            ViewFactory.Register<DeleteItemViewModel, DeleteItemView>();
            ViewFactory.Register<ItemInformationViewModel, ItemInformationView>();
            ViewFactory.Register<UpdateEmailViewModel, UpdateEmailView>();
        }

        public static T Resolve<T>(params Parameter[] parameters)
        {
            if (container == null)
            {
                throw new Exception("Bootstrapper hasn't been started!");
            }

            return container.Resolve<T>(parameters);
        }

        public static T Resolve<T>()
        {
            if (container == null)
            {
                throw new Exception("Bootstrapper hasn't been started!");
            }

            return container.Resolve<T>(new Parameter[0]);
        }
    }
}