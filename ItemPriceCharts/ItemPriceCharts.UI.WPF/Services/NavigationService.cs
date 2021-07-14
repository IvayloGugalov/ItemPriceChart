using System;

using ItemPriceCharts.UI.WPF.ViewModels;

namespace ItemPriceCharts.UI.WPF.Services
{
    public class NavigationService<TViewModel> : INavigationService<TViewModel>
        where TViewModel : BindableViewModel
    {
        private readonly TViewModel getViewModel;
        private BindableViewModel currentViewModel;

        public BindableViewModel CurrentViewModel
        {
            get => this.currentViewModel;
            set
            {
                this.currentViewModel = value;
                this.OnCurrentViewModelChanged();
            }
        }

        public event EventHandler CurrentViewModelChanged;

        public NavigationService(TViewModel getViewModel)
        {
            this.getViewModel = getViewModel;
        }

        public void Navigate()
        {
            this.CurrentViewModel = this.getViewModel;
        }

        private void OnCurrentViewModelChanged()
        {
            this.CurrentViewModelChanged?.Invoke(this, new EventArgs());
        }
    }
}
