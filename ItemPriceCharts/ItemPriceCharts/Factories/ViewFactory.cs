﻿using System;
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
        private readonly Dictionary<Type, Type> map = new Dictionary<Type, Type>();
        public ILifetimeScope lifetimeScope;

        public ViewFactory(ILifetimeScope lifetimeScope)
        {
            this.lifetimeScope = lifetimeScope;
        }

        public void Register<TViewModel, TView>()
            where TViewModel : class, IViewModel
            where TView : Window
        {
            this.map[typeof(TViewModel)] = typeof(TView);
        }

        public Window Resolve<TViewModel>(Parameter[] parameters)
            where TViewModel : class, IViewModel
        {
            TViewModel viewModel = this.lifetimeScope.Resolve<TViewModel>(parameters);

            var viewType = this.map[typeof(TViewModel)];
            var view = this.lifetimeScope.Resolve(viewType) as Window;

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
            TViewModel viewModel = this.lifetimeScope.Resolve<TViewModel>();
            var viewType = this.map[typeof(TViewModel)];
            var view = this.lifetimeScope.Resolve(viewType) as UserControl;

            view.DataContext = viewModel;

            return view;
        }
    }
}
