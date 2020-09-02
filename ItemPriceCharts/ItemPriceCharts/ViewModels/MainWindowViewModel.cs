using System.Windows.Input;

using ItemPriceCharts.UI.WPF.CommandHelpers;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class MainWindowViewModel : BindableViewModel
    {
        private readonly PCShopViewModel shopViewModelPC;
        private readonly PhoneShopViewModel shopViewModelPhone;

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

        public ICommand ShowPCPartsCommand { get; }
        public ICommand ShowPhonesCommand { get; }
        public ICommand ClearViewCommand { get; }

        public MainWindowViewModel(PCShopViewModel shopViewModelPC, PhoneShopViewModel shopViewModelPhone)
        {
            this.shopViewModelPC = shopViewModelPC;
            this.shopViewModelPhone = shopViewModelPhone;
            this.currentView = this;

            this.ShowPCPartsCommand = new RelayCommand(_ => this.ShowPCPartsAction());
            this.ShowPhonesCommand = new RelayCommand(_ => this.ShowPhonesAction());
            this.ClearViewCommand = new RelayCommand(_ => this.ClearViewAction());
        }

        private void ClearViewAction()
        {
            this.IsNewViewDisplayed = false;
        }

        private void ShowPCPartsAction()
        {
            this.CurrentView = this.shopViewModelPC;
            this.IsNewViewDisplayed = true;
        }

        private void ShowPhonesAction()
        {
            this.CurrentView = this.shopViewModelPhone;
            this.IsNewViewDisplayed = true;
        }
    }
}
