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
    public class Bootstrapper : AutofacBootstrapper
    {
        private readonly App app;
        private IViewFactory viewFactory;

        public Bootstrapper(App app)
        {
            this.app = app;

            UIEvents.ShowCreateShopViewModel.Subscribe(this.CreateShopWindow);
            UIEvents.ShowDeleteShopViewModel.Subscribe(this.DeleteShopWindow);
            UIEvents.ShowCreateItemViewModel.Subscribe(this.CreateItemWindow);
            UIEvents.ShowItemInformatioViewModel.Subscribe(this.CreateItemInformationWindow);
            UIEvents.ShowMessageDialog = (vm) =>
            {
                return System.Windows.Application.Current.Dispatcher.Invoke(() => new MessageDialog(vm).ShowDialog());
            };
        }

        public void Stop()
        {
            this.viewFactory.LifetimeScope.Dispose();
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);
            builder.RegisterModule<MainModule>();
            builder.RegisterModule<ViewModelsModule>();
        }

        protected override void ConfigureApplication(IContainer container)
        {
            this.viewFactory = container.Resolve<IViewFactory>();
            var mainWindow = this.viewFactory.Resolve<MainWindowViewModel>(System.Array.Empty<Parameter>());

            this.app.MainWindow = mainWindow;
            this.app.MainWindow.Show();
        }

        protected override void RegisterViews(IViewFactory viewFactory)
        {
            viewFactory.Register<MainWindowViewModel, MainWindow>();

            viewFactory.RegisterUserControl<ItemListingViewModel, ShopsAndItemListingsView>();
            viewFactory.RegisterUserControl<ShopsAndItemListingsViewModel, ShopsAndItemListingsView>();

            viewFactory.Register<CreateShopViewModel, CreateShopView>();
            viewFactory.Register<DeleteShopViewModel, DeleteShopView>();

            viewFactory.Register<CreateItemViewModel, CreateItemView>();
            viewFactory.Register<DeleteItemViewModel, DeleteItemView>();
            viewFactory.Register<ItemInformationViewModel, ItemInformationView>();
        }

        protected void CreateShopWindow(object sender, object e)
        {
            var window = this.viewFactory.Resolve<CreateShopViewModel>(System.Array.Empty<Parameter>());
            window.ShowDialog();
        }

        protected void DeleteShopWindow(object sender, OnlineShop e)
        {
            var parameters = new Parameter[] { new NamedParameter("selectedShop", e) };
            var window = this.viewFactory.Resolve<DeleteShopViewModel>(parameters);
            window.ShowDialog();
        }

        protected void CreateItemWindow(object sender, OnlineShop e)
        {
            var parameters = new Parameter[] { new NamedParameter("selectedShop", e) };
            var window = this.viewFactory.Resolve<CreateItemViewModel>(parameters);
            window.ShowDialog();
        }

        protected void CreateItemInformationWindow(object sender, Item e)
        {
            if (e != null)
            {
                var parameters = new Parameter[] { new NamedParameter("item", e) };
                var window = this.viewFactory.Resolve<ItemInformationViewModel>(parameters);
                window.ShowDialog();
            }
        }
    }
}