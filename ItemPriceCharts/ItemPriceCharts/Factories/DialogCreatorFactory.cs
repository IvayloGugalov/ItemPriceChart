using System;
using System.Windows;

using Autofac;
using Autofac.Core;

using ItemPriceCharts.Services.Models;
using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.UI.WPF.ViewModels;
using ItemPriceCharts.UI.WPF.Views;

namespace ItemPriceCharts.UI.WPF.Factories
{
    public class DialogCreatorFactory
    {
        private readonly IViewFactory viewFactory;

        public DialogCreatorFactory(IViewFactory viewFactory)
        {
            this.viewFactory = viewFactory;

            UIEvents.AddSubscribers();

            UIEvents.ShowCreateShopViewModel.Subscribe(this.CreateShopWindow);
            UIEvents.ShowDeleteShopViewModel.Subscribe(this.DeleteShopWindow);
            UIEvents.ShowCreateItemViewModel.Subscribe(this.CreateItemWindow);
            UIEvents.ShowItemInformatioViewModel.Subscribe(this.CreateItemInformationWindow);
            UIEvents.ShowMessageDialog = (vm) =>
            {
                return Application.Current.Dispatcher.Invoke(() => new MessageDialog(vm).ShowDialog());
            };

            UIEvents.FinishSubscribing();
        }

        private void CreateShopWindow(object sender, object e)
        {
            var window = this.viewFactory.Resolve<CreateShopViewModel>(Array.Empty<Parameter>());
            window.ShowDialog();
        }

        private void DeleteShopWindow(object sender, OnlineShop e)
        {
            var parameters = new Parameter[] { new NamedParameter("selectedShop", e) };
            var window = this.viewFactory.Resolve<DeleteShopViewModel>(parameters);
            window.ShowDialog();
        }

        private void CreateItemWindow(object sender, OnlineShop e)
        {
            var parameters = new Parameter[] { new NamedParameter("selectedShop", e) };
            var window = this.viewFactory.Resolve<CreateItemViewModel>(parameters);
            window.ShowDialog();
        }

        private void CreateItemInformationWindow(object sender, Item e)
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
