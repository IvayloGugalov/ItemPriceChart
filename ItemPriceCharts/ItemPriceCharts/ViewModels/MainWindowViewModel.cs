using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

using System.Linq;
using System.Text;

using ItemPriceCharts.Helpers;

namespace UI.WPF.ViewModels
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

        private async void ShowPCPartsAction()
        {
            await Task.Run(() => this.CurrentView = this.shopViewModelPC);
            this.IsNewViewDisplayed = true;
        }

        private async void ShowPhonesAction()
        {
            await Task.Run(() => this.CurrentView = this.shopViewModelPhone);
            this.IsNewViewDisplayed = true;
        }
    }
}
