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
        }
    }
}
