using System;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using Autofac.Core;
using ItemPriceCharts.UI.WPF.ViewModels;

namespace ItemPriceCharts.UI.WPF.Factories
{
    public interface IViewFactory
    {
        ILifetimeScope LifetimeScope { get; }

        void Register<TViewModel, TView>()
            where TViewModel : class, IViewModel
            where TView : Window;

        Window Resolve<TViewModel>(Parameter[] parameters)
            where TViewModel : class, IViewModel;

        void RegisterUserControl<TViewModel, TView>()
            where TViewModel : class, IViewModel
            where TView : UserControl;

        UserControl ResolveUserControl<TViewModel>()
           where TViewModel : class, IViewModel;
    }
}
