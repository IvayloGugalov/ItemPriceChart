using System;

using ItemPriceCharts.UI.WPF.ViewModels;

namespace ItemPriceCharts.UI.WPF.Services
{
    public interface INavigationService<TViewModel>
        where TViewModel : BindableViewModel
    {
        BindableViewModel CurrentViewModel { get; }

        event EventHandler CurrentViewModelChanged;

        void Navigate();
    }
}