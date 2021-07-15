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

        public DialogCreatorFactory(IViewFactory viewFactory, IDispatcherWrapper dispatcherWrapper)
        {
            this.viewFactory = viewFactory;

            UiEvents.ShowCreateShopView.Register(this.CreateShopView);
            UiEvents.ShowCreateItemView.Register(this.CreateItemView);
            UiEvents.ShowDeleteItemView.Register(this.CreateDeleteItemView);
            UiEvents.ShowItemInformationView.Register(this.CreateItemInformationView);

            UiEvents.ShowMessageDialog = vm => dispatcherWrapper.Invoke(() => new MessageDialog(vm).ShowDialog());
        }

        private void CreateDeleteItemView(Item item)
        {
            var parameters = new Parameter[] { new TypedParameter(typeof(Item), item) };
            var window = this.viewFactory.Resolve<DeleteItemViewModel>(parameters);
            window.ShowDialog();
        }

        private void CreateShopView(UserAccount user)
        {
            var parameters = new Parameter[] { new TypedParameter(typeof(UserAccount), user) };
            var window = this.viewFactory.Resolve<CreateShopViewModel>(parameters);
            window.ShowDialog();
        }

        private void CreateItemView(OnlineShop shop)
        {
            var parameters = new Parameter[] { new TypedParameter(typeof(OnlineShop), shop) };
            var window = this.viewFactory.Resolve<CreateItemViewModel>(parameters);
            window.ShowDialog();
        }

        private void CreateItemInformationView(Item item)
        {
            if (item != null)
            {
                var parameters = new Parameter[] { new TypedParameter(typeof(Item), item) };
                var window = this.viewFactory.Resolve<ItemInformationViewModel>(parameters);
                window.ShowDialog();
            }
        }
    }
}
