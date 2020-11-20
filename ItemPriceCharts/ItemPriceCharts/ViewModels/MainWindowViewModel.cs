using System.Windows.Input;

using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Helpers;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class MainWindowViewModel : BindableViewModel
    {
        private readonly ShopsAndItemListingsViewModel shopsAndItemListingsViewModel;
        private readonly ItemListingViewModel itemListingViewModel;

        private object currentView;
        private bool isNewViewDisplayed;

        public bool IsNewViewDisplayed
        {
            get => this.isNewViewDisplayed;
            set => this.SetValue(ref this.isNewViewDisplayed, value);
        }

        public object CurrentView
        {
            get => this.currentView;
            set => this.SetValue(ref this.currentView, value);
        }

        public ICommand ShowShopsAndItemListingsCommand { get; }
        public ICommand ShowItemListingCommand { get; }
        public ICommand ClearViewCommand { get; }
        public ICommand ShowLogFileCommand { get; }

        public MainWindowViewModel(ShopsAndItemListingsViewModel shopsAndItemListingsViewModel, ItemListingViewModel itemListingViewModel)
        {
            this.shopsAndItemListingsViewModel = shopsAndItemListingsViewModel;
            this.itemListingViewModel = itemListingViewModel;
            this.currentView = this;

            this.ShowShopsAndItemListingsCommand = new RelayCommand(_ => this.ShowShopsAndItemListingsAction());
            this.ShowItemListingCommand = new RelayCommand(_ => this.ShowItemListingAction());
            this.ClearViewCommand = new RelayCommand(_ => this.ClearViewAction());
            this.ShowLogFileCommand = new RelayCommand(_ => LogHelper.OpenLogFolder());
        }

        private void ClearViewAction()
        {
            this.IsNewViewDisplayed = false;
        }

        private void ShowShopsAndItemListingsAction()
        {
            this.CurrentView = this.shopsAndItemListingsViewModel;
            this.IsNewViewDisplayed = true;
        }

        private void ShowItemListingAction()
        {
            this.CurrentView = this.itemListingViewModel;
            this.IsNewViewDisplayed = true;
        }
    }
}
