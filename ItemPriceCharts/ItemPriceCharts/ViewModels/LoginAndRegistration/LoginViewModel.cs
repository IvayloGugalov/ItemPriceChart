using System;
using System.Threading.Tasks;
using System.Windows.Input;

using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.XmReaderWriter.User;

namespace ItemPriceCharts.UI.WPF.ViewModels.LoginAndRegistration
{
    public class LoginViewModel : UserCredentialForm
    {
        private readonly IUserAccountService userAccountService;

        private BindableViewModel currentViewModel;
        private RegisterViewModel registerViewModel;
        private bool rememberUser = true;
        private bool loginHasExpired;

        public bool CloseTrigger { get; set; }

        public BindableViewModel CurrentViewModel
        {
            get => this.currentViewModel;
            set => this.SetValue(ref this.currentViewModel, value);
        }

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

        public IAsyncCommand LoginCommand { get; }
        public ICommand ShowRegisterViewCommand { get; }
        public override ICommand ClosedCommand => new RelayCommand(_ => this.ClosedCommandAction());

        public LoginViewModel(IUserAccountService userAccountService, string userName, string email)
        {
            this.userAccountService = userAccountService ?? throw new ArgumentNullException(nameof(userAccountService));

            this.Username = userName;
            this.Email = email;

            this.LoginCommand = new RelayAsyncCommand(this.LoginCommandAction, this.LoginCommandPredicate, errorHandler: e =>
            {
                MessageDialogCreator.ShowErrorDialog(message: e.Message);
            });

            this.ShowRegisterViewCommand = new RelayCommand(this.ShowRegisterViewAction);
            this.CurrentViewModel = this;
        }

        private void ShowRegisterViewAction(object param)
        {
            if (this.registerViewModel is null)
            {
                this.registerViewModel = new RegisterViewModel(this.userAccountService, this);
            }

            this.CurrentViewModel = this.registerViewModel;
        }

        private async Task LoginCommandAction()
        {
            var (loginResult, _) = await this.userAccountService.TryGetUserAccount(this.Username, this.Email, this.Password);

            if (loginResult == UserAccountLoginResult.SuccessfullyLogin)
            {
                if (this.RememberUser)
                {
                    await this.WriteUserCredentials();
                }

                this.CloseTrigger = true;
                this.OnPropertyChanged(nameof(this.CloseTrigger));
            }
            else
            {
                this.ErrorMessage = LoginViewModel.GetLoginErrorMessage(loginResult);
            }
        }

        private async Task WriteUserCredentials()
        {
            try
            {
                await Task.Run(() =>
                {
                    UserCredentialsSettings.Username = this.Username;
                    UserCredentialsSettings.Email = this.Email;
                    UserCredentialsSettings.RememberAccount = this.RememberUser.ToString();
                    UserCredentialsSettings.LoginExpiresDate = DateTime.Now.AddMinutes(30).ToString();
                    UserCredentialsSettings.WriteToXmlFile();
                });
            }
            catch (Exception e)
            {
                MessageDialogCreator.ShowErrorDialog(e.Message);
            }
        }

        private static string GetLoginErrorMessage(UserAccountLoginResult loginResult) => loginResult switch
        {
            UserAccountLoginResult.SuccessfullyLogin => string.Empty,
            UserAccountLoginResult.InvalidUsernameOrEmail => "Invalid username or email.",
            UserAccountLoginResult.InvalidPassword => "Invalid password.",
            _ => "Unanticipated error"
        };

        private bool LoginCommandPredicate() =>
            base.AreCredentialsFilled() &&
            base.IsEmailValid;

        /// <summary>
        /// Close the application only when the user requested it
        /// </summary>
        /// Closing when the registration control is used is handled here as well.
        private void ClosedCommandAction()
        {
            if (!this.CloseTrigger)
            {
                UIEvents.CloseApplication();
            }
        }
    }
}
