using Autofac;

using UI.WPF.ViewModels;
using UI.WPF.Views;

namespace UI.WPF.Modules
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
