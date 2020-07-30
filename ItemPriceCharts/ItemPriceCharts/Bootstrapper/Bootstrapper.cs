using Autofac;


using ItemPriceCharts.UI.WPF.Factories;
using ItemPriceCharts.UI.WPF.Modules;
using ItemPriceCharts.UI.WPF.ViewModels;
using ItemPriceCharts.UI.WPF.Views;

namespace ItemPriceCharts.UI.WPF.Bootstrapper
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
