using System;
using System.Collections.Generic;

using Autofac;

namespace ItemPriceCharts.UI.WPF.Modules
{
    public class MappedTypeModules : Module
    {
        private readonly Dictionary<Type, Type> mappedTypes;

        public MappedTypeModules(Dictionary<Type, Type> mappedTypes)
        {
            this.mappedTypes = mappedTypes;
        }

        protected override void Load(ContainerBuilder builder)
        {
            foreach (var item in this.mappedTypes)
            {
                builder.RegisterType(item.Value).AsSelf().As(item.Key).SingleInstance();
            }
        }
    }
}
