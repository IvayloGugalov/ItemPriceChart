using Autofac;

using ItemPriceCharts.UI.WPF.ViewModels;
using ItemPriceCharts.UI.WPF.Views;

namespace ItemPriceCharts.UI.WPF.Modules
{
    public class ViewModelsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ShopView>();
            builder.RegisterType<PCShopViewModel>();
            builder.RegisterType<PhoneShopViewModel>();

            builder.RegisterType<CreateShopViewModel>();
            builder.RegisterType<CreateShopView>();

            builder.RegisterType<DeleteShopViewModel>();
            builder.RegisterType<DeleteShopView>();

            builder.RegisterType<CreateItemViewModel>();
            builder.RegisterType<CreateItemView>();

            builder.RegisterType<DeleteItemViewModel>();
            builder.RegisterType<DeleteItemView>();
        }
    }
}
