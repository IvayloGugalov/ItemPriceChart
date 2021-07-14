using Autofac;

using ItemPriceCharts.UI.WPF.Events;
using ItemPriceCharts.UI.WPF.Factories;

namespace ItemPriceCharts.UI.WPF.Modules
{
    public class SelfTypeModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DialogCreatorFactory>()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<UiEvents>()
                .AsSelf()
                .SingleInstance();
        }
    }
}
