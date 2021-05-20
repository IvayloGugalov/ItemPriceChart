using Autofac;

using ItemPriceCharts.UI.WPF.Factories;

namespace ItemPriceCharts.UI.WPF.Modules
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ViewFactory>().As<IViewFactory>().SingleInstance();
        }
    }
}
