using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

using Autofac;
using Autofac.Core;

using ItemPriceCharts.UI.WPF.ViewModels;

namespace ItemPriceCharts.UI.WPF.Factories
{
    public class ViewFactory : IViewFactory
    {
        private readonly Dictionary<Type, Type> map = new();

        public ILifetimeScope LifetimeScope { get; }

        public ViewFactory(ILifetimeScope lifetimeScope)
        {
            this.LifetimeScope = lifetimeScope;
        }

        public void Register<TViewModel, TView>()
            where TViewModel : class, IViewModel
            where TView : Window
        {
            this.map[typeof(TViewModel)] = typeof(TView);
        }

        public Window Resolve<TViewModel>(Parameter[] parameters = null)
            where TViewModel : class, IViewModel
        {
            var viewModel = this.LifetimeScope.Resolve<TViewModel>(parameters ?? Array.Empty<Parameter>());

            var viewType = this.map[typeof(TViewModel)];
            var view = this.LifetimeScope.Resolve(viewType) as Window;

            view.DataContext = viewModel;

            return view;
        }

        public void RegisterUserControl<TViewModel, TView>()
            where TViewModel : class, IViewModel
            where TView : UserControl
        {
            this.map[typeof(TViewModel)] = typeof(TView);
        }

        public UserControl ResolveUserControl<TViewModel>()
            where TViewModel : class, IViewModel
        {
            var viewModel = this.LifetimeScope.Resolve<TViewModel>();
            var viewType = this.map[typeof(TViewModel)];
            var view = this.LifetimeScope.Resolve(viewType) as UserControl;

            view.DataContext = viewModel;

            return view;
        }
    }
}
