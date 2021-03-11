using Autofac;

using ItemPriceCharts.UI.WPF.ViewModels;
using ItemPriceCharts.UI.WPF.Views;

namespace ItemPriceCharts.UI.WPF.Modules
{
    public class MainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MainWindowViewModel>().SingleInstance();
            builder.RegisterType<MainWindow>().SingleInstance();
            builder.RegisterType<LoginViewModel>().InstancePerLifetimeScope();
            builder.RegisterType<LoginView>().InstancePerLifetimeScope();
        }
    }
}
