using Autofac;

using ItemPriceCharts;
using UI.WPF.Factories;
using UI.WPF.Modules;
using UI.WPF.ViewModels;
using UI.WPF.Views;

namespace UI.WPF.Bootstrapper
{
    public class Bootstrapper : AutofacBootstrapper
    {
        private readonly App app;

        public Bootstrapper(App app)
        {
            this.app = app;
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);
            builder.RegisterModule<MainModule>();
            builder.RegisterModule<ViewModelsModule>();
        }

        protected override void ConfigureApplication(IContainer container)
        {
            var viewFactory = container.Resolve<IViewFactory>();
            var mainWindow = viewFactory.Resolve<MainWindowViewModel>();
            this.app.MainWindow = mainWindow;
            this.app.MainWindow.Show();
        }

        protected override void RegisterViews(IViewFactory viewFactory)
        {
            viewFactory.Register<MainWindowViewModel, MainWindow>();
            viewFactory.RegisterUserControl<PhoneShopViewModel, ShopView>();
            viewFactory.RegisterUserControl<PCShopViewModel, ShopView>();
        }
    }
}
