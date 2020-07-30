using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;

using ItemPriceCharts.UI.WPF.Factories;
using ItemPriceCharts.UI.WPF.Modules;

namespace ItemPriceCharts.UI.WPF.Bootstrapper
{
    public abstract class AutofacBootstrapper
    {
        private Dictionary<Type, Type> mappedTypes;

        public void Run(Dictionary<Type, Type> mappedTypes)
        {
            this.mappedTypes = mappedTypes;

            var builder = new ContainerBuilder();

            this.ConfigureContainer(builder);

            var container = builder.Build();

            var viewFactory = container.Resolve<IViewFactory>();
            this.RegisterViews(viewFactory);
            this.ConfigureApplication(container);
        }

        protected virtual void ConfigureContainer(ContainerBuilder builder)
        {
            if (this.mappedTypes != null && this.mappedTypes.Any())
            {
                builder.RegisterModule(new MappedTypeModules(this.mappedTypes));
            }

            builder.RegisterModule<AutofacModule>();
        }

        protected abstract void RegisterViews(IViewFactory viewFactory);
        protected abstract void ConfigureApplication(IContainer container);
    }
}
