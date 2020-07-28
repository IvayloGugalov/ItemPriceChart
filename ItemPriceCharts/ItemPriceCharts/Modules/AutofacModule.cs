using System;
using System.Collections.Generic;
using System.Text;

using Autofac;
using Services.Data;
using Services.Models;
using Services.Services;
using Services.Strategies;
using UI.WPF.Factories;

namespace UI.WPF.Modules
{
    public class AutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ViewFactory>().As<IViewFactory>().SingleInstance();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
        }
    }
}
