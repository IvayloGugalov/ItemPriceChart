using Autofac;

using ItemPriceCharts.UI.WPF.ViewModels;
using ItemPriceCharts.UI.WPF.Views;

namespace ItemPriceCharts.UI.WPF.Modules
{
    public class ViewModelsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ShopsAndItemListingsViewModel>();
            builder.RegisterType<ItemListingViewModel>();

            builder.RegisterType<CreateShopViewModel>();
            builder.RegisterType<CreateShopView>();

            builder.RegisterType<CreateItemViewModel>();
            builder.RegisterType<CreateItemView>();

            builder.RegisterType<DeleteItemViewModel>();
            builder.RegisterType<DeleteItemView>();

            builder.RegisterType<ItemInformationViewModel>();
            builder.RegisterType<ItemInformationView>();
        }
    }
}
