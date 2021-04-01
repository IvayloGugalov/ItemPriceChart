using Autofac;

using ItemPriceCharts.Services.Data;
using ItemPriceCharts.UI.WPF.Factories;

namespace ItemPriceCharts.UI.WPF.Modules
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).SingleInstance();
            builder.RegisterType<ViewFactory>().As<IViewFactory>().SingleInstance();
        }
    }
}
