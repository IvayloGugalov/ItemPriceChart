using System;
using System.Threading.Tasks;
using System.Windows.Input;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.Infrastructure.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Events;
using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.UI.WPF.Services;

namespace ItemPriceCharts.UI.WPF.ViewModels.LoginAndRegistration
{
    public class LoginViewModel : UserCredentialForm
    {
        private const string USERNAME_NOT_EXISTING_MESSAGE = "Username does not exist.";
        private const string EMAIL_NOT_EXISTING_MESSAGE = "Email does not exist.";
        private const string INVALID_PASSWORD_MESSAGE = "Password is invalid.";

        private readonly IUserAccountService userAccountService;

        private bool rememberUser = true;
        private bool loginHasExpired;

        public bool RememberUser
        {
            get => this.rememberUser;
            set => this.SetValue(ref this.rememberUser, value);
        }

        public bool LoginHasExpired
        {
            get => this.loginHasExpired;
            set => this.SetValue(ref this.loginHasExpired, value);
        }

        public UserAccount UserAccount { get; private set; }


        /// <summary>
        /// Circular Dependency between the view models. Don't change this property as it will break the DI!
        /// </summary>
        public INavigationService<RegisterViewModel> NavigationService { get; set; }

        public IAsyncCommand LoginCommand { get; }
        public ICommand ShowRegisterViewCommand { get; }

        public LoginViewModel(IUserAccountService userAccountService, string userName, string email)
        {
            this.userAccountService = userAccountService ?? throw new ArgumentNullException(nameof(userAccountService));

            this.Username = userName;
            this.Email = email;

            this.LoginCommand = new RelayAsyncCommand(this.LoginCommandAction, this.LoginCommandPredicate, errorHandler: e =>
            {
                MessageDialogCreator.ShowErrorDialog(message: e.Message);
            });

            this.ShowRegisterViewCommand = new RelayCommand(_ => this.NavigationService.Navigate());
        }

        private async Task LoginCommandAction()
        {
            var (loginResult, userAccount) = await this.userAccountService.TryGetUserAccount(this.Username, this.Email, this.Password);

            if (loginResult == UserAccountLoginResult.SuccessfulLogin)
            {
                // Write credentials directly into TryGetUserAccount??
                if (this.RememberUser)
                {
                    await this.userAccountService.WriteUserCredentials(userAccount, this.RememberUser, DateTime.UtcNow.AddDays(30).ToString());
                }

                UiEvents.SuccessfulLogin.Raise(userAccount);
            }
            else
            {
                this.ErrorMessage = LoginViewModel.GetLoginErrorMessage(loginResult);
            }
        }

        private static string GetLoginErrorMessage(UserAccountLoginResult loginResult) => loginResult switch
        {
            UserAccountLoginResult.InvalidUsername => USERNAME_NOT_EXISTING_MESSAGE,
            UserAccountLoginResult.InvalidEmail => EMAIL_NOT_EXISTING_MESSAGE,
            UserAccountLoginResult.InvalidPassword => INVALID_PASSWORD_MESSAGE,
            UserAccountLoginResult.SuccessfulLogin => string.Empty,
            _ => "Unanticipated error"
        };

        private bool LoginCommandPredicate() =>
            base.AreCredentialsFilled() &&
            base.IsEmailValid;
    }
}
