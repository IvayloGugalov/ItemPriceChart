using System.Windows;
using System.Windows.Controls;

using UI.WPF.ViewModels;

namespace UI.WPF.Factories
{
    public interface IViewFactory
    {
        void Register<TViewModel, TView>()
            where TViewModel : class, IViewModel
            where TView : Window;

        Window Resolve<TViewModel>()
           where TViewModel : class, IViewModel;

        void RegisterUserControl<TViewModel, TView>()
            where TViewModel : class, IViewModel
            where TView : UserControl;

        UserControl ResolveUserControl<TViewModel>()
           where TViewModel : class, IViewModel;
    }
}
