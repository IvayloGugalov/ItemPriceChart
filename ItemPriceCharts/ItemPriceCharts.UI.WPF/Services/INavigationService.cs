using System;

using ItemPriceCharts.UI.WPF.ViewModels.Base;

namespace ItemPriceCharts.UI.WPF.Services
{
    public interface INavigationService<TViewModel>
        where TViewModel : BaseViewModel
    {
        BaseViewModel CurrentViewModel { get; }

        event EventHandler CurrentViewModelChanged;

        void Navigate();
    }
}