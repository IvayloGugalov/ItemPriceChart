using System.Threading.Tasks;
using System.Windows.Input;

using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Helpers;

namespace ItemPriceCharts.UI.WPF.ViewModels.LoginAndRegistration
{
    public class RegisterViewModel : UserCredentialForm
    {
        private readonly IUserAccountService userAccountService;
        private readonly LoginViewModel loginViewModel;

        private string confirmPassword;
        private string firstName = "Ivaylo";
        private string lastName = "Gugalov";

        public string FirstName
        {
            get => this.firstName;
            set => this.SetValue(ref this.firstName, value);
        }

        public string LastName
        {
            get => this.lastName;
            set => this.SetValue(ref this.lastName, value);
        }

        public string ConfirmPassword
        {
            get => this.confirmPassword;
            set
            {
                this.ErrorMessage = string.Empty;
                this.SetValue(ref this.confirmPassword, value);
            }
        }

        public IAsyncCommand RegisterCommand { get; }
        public ICommand MoveBackCommand { get; }

        public RegisterViewModel(IUserAccountService userAccountService, LoginViewModel loginViewModel)
        {
            this.userAccountService = userAccountService;
            this.loginViewModel = loginViewModel;

            this.Password = "123123";
            this.ConfirmPassword = "123123";
            this.Email = "emai@abv.bg";
            this.Username = "Maslotopz";

            this.RegisterCommand = new RelayAsyncCommand(this.RegisterCommandAction, this.RegisterCommandPredicate, errorHandler: e =>
            {
                MessageDialogCreator.ShowErrorDialog(message: e.Message);
            });

            this.MoveBackCommand = new RelayCommand(this.MoveBackCommandAction);
        }

        private void MoveBackCommandAction(object param)
        {
            this.loginViewModel.CurrentViewModel = this.loginViewModel;
        }

        private async Task RegisterCommandAction()
        {
            await this.CreateUserAccount();
        }

        private async Task CreateUserAccount()
        {
            var userAccountCreationResult = await this.userAccountService.CreateUserAccount(
                firstName: this.FirstName,
                lastName: this.LastName,
                userName: this.Username,
                email: this.Email,
                password: this.Password);

            if (userAccountCreationResult == UserAccountRegistrationResult.UserAccountCreated)
            {
                this.MoveBackCommandAction(null);
                return;
            }

            this.ErrorMessage = RegisterViewModel.GetCredentialsErrorMessage(userAccountCreationResult);
        }

        private static string GetCredentialsErrorMessage(UserAccountRegistrationResult errorType) => errorType switch
        {
            UserAccountRegistrationResult.EmailAlreadyExists => "Email already exists.",
            UserAccountRegistrationResult.InvalidPassword => "Invalid password.",
            UserAccountRegistrationResult.UserNameAlreadyExists => "Username already exists.",
            UserAccountRegistrationResult.CanNotCreateUserAccount => "Can't create account.",
            UserAccountRegistrationResult.UserAccountCreated => string.Empty,
            _ => "Unanticipated error"
        };

        private bool RegisterCommandPredicate() =>
            this.AreCredentialsFilled() &&
            this.ArePasswordsEqual() &&
            base.IsEmailValid;

        public override bool AreCredentialsFilled() =>
            !string.IsNullOrWhiteSpace(this.FirstName) &&
            !string.IsNullOrWhiteSpace(this.LastName) &&
            !string.IsNullOrWhiteSpace(this.ConfirmPassword) &&
            base.AreCredentialsFilled();

        private bool ArePasswordsEqual()
        {
            var areEqualPasswords = this.Password == this.ConfirmPassword;
            if (!areEqualPasswords)
            {
                this.ErrorMessage = "Passwords do not match";
            }

            return areEqualPasswords;
        }
    }
}
