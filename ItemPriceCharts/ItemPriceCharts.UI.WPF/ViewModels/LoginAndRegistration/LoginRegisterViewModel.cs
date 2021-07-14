using System.Windows.Input;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Events;
using ItemPriceCharts.UI.WPF.Services;

namespace ItemPriceCharts.UI.WPF.ViewModels.LoginAndRegistration
{
    public class LoginRegisterViewModel : BindableViewModel
    {
        private readonly INavigationService<LoginViewModel> navigateToLoginService;
        private readonly INavigationService<RegisterViewModel> navigateToRegisterService;

        private BindableViewModel currentViewModel;

        public BindableViewModel CurrentViewModel
        {
            get => this.currentViewModel;
            private set => this.SetValue(ref this.currentViewModel, value);
        }

        /// <summary>
        /// Binds to the CloseWindowBehavior on the view.
        /// Invokes this.ClosedCommandAction when the Closed event is raised.
        /// /// </summary>
        public bool SuccessfulLogin { get; private set; }

        public ICommand ClosedCommand => new RelayCommand(_ => this.ClosedCommandAction());

        public LoginRegisterViewModel(INavigationService<LoginViewModel> navigateToLoginService, INavigationService<RegisterViewModel> navigateToRegisterService)
        {
            this.navigateToLoginService = navigateToLoginService;
            this.navigateToRegisterService = navigateToRegisterService;

            this.navigateToLoginService.CurrentViewModelChanged += this.OnCurrentViewModelChanged;
            this.navigateToRegisterService.CurrentViewModelChanged += this.OnCurrentViewModelChanged;

            this.navigateToLoginService.Navigate();

            UiEvents.SuccessfulLogin.Register(_ =>
            {
                this.SuccessfulLogin = true;
                this.OnPropertyChanged(() => this.SuccessfulLogin);
            });
        }

        private void OnCurrentViewModelChanged(object sender, object parameter)
        {
            this.CurrentViewModel = this.CurrentViewModel is LoginViewModel
                ? this.navigateToRegisterService.CurrentViewModel
                : this.navigateToLoginService.CurrentViewModel;
        }

        public UserAccount LoggedUserAccount()
        {
            if (this.CurrentViewModel is LoginViewModel loginViewModel && loginViewModel.UserAccount != null)
            { 
                return loginViewModel.UserAccount;
            }

            return null;
        }

        /// <summary>
        /// Close the application only when the user requested it
        /// Closing when the registration control is handled here as well.
        /// </summary>
        private void ClosedCommandAction()
        {
            if (!this.SuccessfulLogin)
            {
                UiEvents.CloseApplication();
            }
        }
    }
}
