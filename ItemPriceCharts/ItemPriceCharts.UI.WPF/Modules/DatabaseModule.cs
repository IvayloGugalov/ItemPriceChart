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
                .InstancePerLifetimeScope()
                .OnRelease(instance => instance.Dispose());

            builder.Register<ModelsContext>(c => c.Resolve<ModelsContextFactory>().CreateDbContext())
                .InstancePerLifetimeScope()
                .OnRelease(instance => instance.Dispose());
        }
    }
}
