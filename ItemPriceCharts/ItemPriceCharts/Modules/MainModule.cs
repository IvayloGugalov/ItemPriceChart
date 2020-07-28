using System;
using System.Collections.Generic;
using System.Text;

using Autofac;
using UI.WPF.ViewModels;
using UI.WPF.Views;

namespace UI.WPF.Modules
{
    public class MainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MainWindowViewModel>().SingleInstance();
            builder.RegisterType<MainWindow>().SingleInstance();
        }
    }
}
