using Autofac;

using ItemPriceCharts.Infrastructure.Data;

namespace ItemPriceCharts.UI.WPF.Modules
{
    public class DatabaseModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ModelsContextFactory>()
                .AsSelf()
                .SingleInstance()
                .OnRelease(instance => instance.Dispose());

            builder.Register<ModelsContext>(c => c.Resolve<ModelsContextFactory>().CreateDbContext())
                .SingleInstance()
                .OnRelease(instance => instance.Dispose());
        }
    }
}
