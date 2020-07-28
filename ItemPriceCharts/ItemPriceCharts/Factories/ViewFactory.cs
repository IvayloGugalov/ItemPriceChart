using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Autofac;
using UI.WPF.ViewModels;

namespace UI.WPF.Factories
{
    public class ViewFactory : IViewFactory
    {
        private readonly Dictionary<Type, Type> map = new Dictionary<Type, Type>();
        private readonly IComponentContext componentContext;

        public ViewFactory(IComponentContext componentContext)
        {
            this.componentContext = componentContext;
        }

        public void Register<TViewModel, TView>()
            where TViewModel : class, IViewModel
            where TView : Window
        {
            this.map[typeof(TViewModel)] = typeof(TView);
        }

        public Window Resolve<TViewModel>()
            where TViewModel : class, IViewModel
        {
            TViewModel viewModel = this.componentContext.Resolve<TViewModel>();
            var viewType = this.map[typeof(TViewModel)];
            var view = this.componentContext.Resolve(viewType) as Window;

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
            TViewModel viewModel = this.componentContext.Resolve<TViewModel>();
            var viewType = this.map[typeof(TViewModel)];
            var view = this.componentContext.Resolve(viewType) as UserControl;

            view.DataContext = viewModel;

            return view;
        }
    }
}
