using System.Windows.Input;

using Autofac;
using Autofac.Core;
using NLog;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Events;
using ItemPriceCharts.UI.WPF.ViewModels.Base;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(MainWindowViewModel));

        private readonly ItemListingViewModel itemListingViewModel;

        private BaseViewModel currentView;
        private bool isNewViewDisplayed;

        public bool IsNewViewDisplayed
        {
            get => this.isNewViewDisplayed;
            set => this.SetValue(ref this.isNewViewDisplayed, value);
        }

        public BaseViewModel CurrentView
        {
            get => this.currentView;
            set => this.SetValue(ref this.currentView, value);
        }

        public UserAccount UserAccount { get; }

        public SideMenuViewModel SideMenuViewModel { get; }

        public ICommand ClosedCommand => new RelayCommand(_ => UiEvents.CloseApplication());

        public MainWindowViewModel(UserAccount userAccount, UiEvents uiEvents)
        {
            this.itemListingViewModel = new ItemListingViewModel(userAccount, uiEvents);

            this.SideMenuViewModel = Bootstrapper.Bootstrapper.Resolve<SideMenuViewModel>(parameters: new Parameter[]
            {
                new TypedParameter(typeof(UserAccount), userAccount),
                new TypedParameter(typeof(UiEvents), uiEvents)
            });
            this.OnPropertyChanged(nameof(this.SideMenuViewModel));

            this.SideMenuViewModel.ShowItems += this.ShowItemListingAction;
            this.SideMenuViewModel.ClearView += this.ClearViewAction;

            this.UserAccount = userAccount;
            this.currentView = this;

        }

        private void ClearViewAction()
        {
            this.CurrentView = this;
            this.IsNewViewDisplayed = false;
        }

        private void ShowItemListingAction(OnlineShop onlineShop)
        {
            this.itemListingViewModel.SelectedShop = onlineShop;

            this.itemListingViewModel.SetItemsList();

            this.CurrentView = this.itemListingViewModel;
            this.IsNewViewDisplayed = true;
        }
    }
}
