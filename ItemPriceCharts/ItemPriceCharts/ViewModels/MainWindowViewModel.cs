using System.Windows.Input;

using ItemPriceCharts.UI.WPF.CommandHelpers;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class MainWindowViewModel : BindableViewModel
    {
        private readonly ShopsViewModel shopViewModelPC;
        private readonly ViewItemsViewModel itemsViewModel;

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

        public MainWindowViewModel(ShopsViewModel shopsViewModel, ViewItemsViewModel shopViewModelPhone)
        {
            this.shopViewModelPC = shopsViewModel;
            this.itemsViewModel = shopViewModelPhone;
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
            this.CurrentView = this.shopViewModelPC;
            this.IsNewViewDisplayed = true;
        }

        private void ShowPhonesAction()
        {
            this.CurrentView = this.itemsViewModel;
            this.IsNewViewDisplayed = true;
        }
    }
}
