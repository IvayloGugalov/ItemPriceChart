using Autofac;
using Autofac.Core;
using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.Factories;
using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.UI.WPF.Modules;
using ItemPriceCharts.UI.WPF.ViewModels;
using ItemPriceCharts.UI.WPF.Views;
using System;

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
            var mainWindow = this.viewFactory.Resolve<MainWindowViewModel>();

            this.app.MainWindow = mainWindow;
            this.app.MainWindow.Show();
        }

        protected override void RegisterViews(IViewFactory viewFactory)
        {
            viewFactory.Register<MainWindowViewModel, MainWindow>();

            viewFactory.RegisterUserControl<PhoneShopViewModel, ShopView>();
            viewFactory.RegisterUserControl<PCShopViewModel, ShopView>();

            viewFactory.Register<CreateShopViewModel, CreateShopView>();
            viewFactory.Register<DeleteShopViewModel, DeleteShopView>();

            viewFactory.Register<CreateItemViewModel, CreateItemView>();
            viewFactory.Register<DeleteItemViewModel, DeleteItemView>();
            viewFactory.Register<ItemInformationViewModel, ItemInformationView>();
        }

        protected void CreateShopWindow(object sender, MessageArgument<object> e)
        {
            var window = this.viewFactory.Resolve<CreateShopViewModel>();
            window.Show();
        }

        protected void DeleteShopWindow(object sender, MessageArgument<OnlineShopModel> e)
        {
            var parameters = new Parameter[] { new NamedParameter("selectedShop", e.Message) };
            var window = this.viewFactory.Resolve<DeleteShopViewModel>(parameters);
            window.Show();
        }

        protected void CreateItemWindow(object sender, MessageArgument<OnlineShopModel> e)
        {
            var parameters = new Parameter[] { new NamedParameter("selectedShop", e.Message) };
            var window = this.viewFactory.Resolve<CreateItemViewModel>(parameters);
            window.Show();
        }

        protected void CreateItemInformationWindow(object sender, MessageArgument<ItemModel> e)
        {
            var parameters = new Parameter[] { new NamedParameter("selectedItem", e.Message) };
            var window = this.viewFactory.Resolve<ItemInformationViewModel>(parameters);
            window.Show();
        }
    }
}