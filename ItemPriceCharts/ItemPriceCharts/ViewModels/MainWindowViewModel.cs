using System.Windows.Input;

using ItemPriceCharts.UI.WPF.CommandHelpers;

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

        public ICommand ShowAllShopsCommand { get; }
        public ICommand ShowPhonesCommand { get; }
        public ICommand ClearViewCommand { get; }

        public MainWindowViewModel(ShopsAndItemListingsViewModel shopsAndItemListingsViewModel, ItemListingViewModel itemListingViewModel)
        {
            this.shopsAndItemListingsViewModel = shopsAndItemListingsViewModel;
            this.itemListingViewModel = itemListingViewModel;
            this.currentView = this;

            this.ShowAllShopsCommand = new RelayCommand(_ => this.ShowAllShopsAction());
            this.ShowPhonesCommand = new RelayCommand(_ => this.ShowPhonesAction());
            this.ClearViewCommand = new RelayCommand(_ => this.ClearViewAction());
        }

        private void ClearViewAction()
        {
            this.IsNewViewDisplayed = false;
        }

        private void ShowAllShopsAction()
        {
            this.CurrentView = this.shopsAndItemListingsViewModel;
            this.IsNewViewDisplayed = true;
        }

        private void ShowPhonesAction()
        {
            this.CurrentView = this.itemListingViewModel;
            this.IsNewViewDisplayed = true;
        }
    }
}
