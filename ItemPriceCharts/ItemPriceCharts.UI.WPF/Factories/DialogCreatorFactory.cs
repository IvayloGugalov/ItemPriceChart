using System;
using System.Threading;
using System.Windows;

using Autofac;
using Autofac.Core;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.UI.WPF.Events;
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

            UiEvents.ShowCreateShopView.Register(this.CreateShopView);
            UiEvents.ShowCreateItemView.Register(this.CreateItemView);
            UiEvents.ShowDeleteItemView.Register(this.CreateDeleteItemView);
            UiEvents.ShowItemInformationView.Register(this.CreateItemInformationView);

            UiEvents.ShowMessageDialog = vm => Application.Current.Dispatcher.Invoke(() => new MessageDialog(vm).ShowDialog());
            UiEvents.ShowLoginRegisterWindow = vm => Application.Current.Dispatcher.Invoke(() => new LoginRegisterView(vm).ShowDialog());
        }

        private void CreateDeleteItemView(Item e)
        {
            var parameters = new Parameter[] { new NamedParameter("item", e) };
            var window = this.viewFactory.Resolve<DeleteItemViewModel>(parameters);
            window.ShowDialog();
        }

        private void CreateShopView(UserAccount user)
        {
            var parameters = new Parameter[] { new NamedParameter("userAccount", user) };
            var window = this.viewFactory.Resolve<CreateShopViewModel>(parameters);
            window.ShowDialog();
        }

        private void CreateItemView(OnlineShop e)
        {
            var parameters = new Parameter[] { new NamedParameter("selectedShop", e) };
            var window = this.viewFactory.Resolve<CreateItemViewModel>(parameters);
            window.ShowDialog();
        }

        private void CreateItemInformationView(Item e)
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
