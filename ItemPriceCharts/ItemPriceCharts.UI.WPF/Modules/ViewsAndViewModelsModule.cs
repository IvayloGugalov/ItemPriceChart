using Autofac;

using ItemPriceCharts.UI.WPF.ViewModels;
using ItemPriceCharts.UI.WPF.ViewModels.LoginAndRegistration;
using ItemPriceCharts.UI.WPF.Views;

namespace ItemPriceCharts.UI.WPF.Modules
{
    public class ViewsAndViewModelsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LoginRegisterViewModel>()
                .AsSelf()
                .SingleInstance();
            builder.RegisterType<LoginRegisterView>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<LoginViewModel>()
                .AsSelf()
                .SingleInstance()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            builder.RegisterType<RegisterViewModel>()
                .AsSelf()
                .SingleInstance()
                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            builder.RegisterType<MainWindowViewModel>().SingleInstance();
            builder.RegisterType<MainWindow>().SingleInstance();

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
