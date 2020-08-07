using System.Windows;
using System.Windows.Controls;

using ItemPriceCharts.UI.WPF.ViewModels;

namespace ItemPriceCharts.UI.WPF.Factories
{
    public interface IViewFactory
    {
        void Register<TViewModel, TView>()
            where TViewModel : class, IViewModel
            where TView : Window;

        Window Resolve<TViewModel>()
           where TViewModel : class, IViewModel;

        Window Resolve(IViewModel viewModel);

        void RegisterUserControl<TViewModel, TView>()
            where TViewModel : class, IViewModel
            where TView : UserControl;

        UserControl ResolveUserControl<TViewModel>()
           where TViewModel : class, IViewModel;
    }
}
