using System.Threading.Tasks;
using System.Windows.Input;

using ItemPriceCharts.Infrastructure.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.UI.WPF.Services;

namespace ItemPriceCharts.UI.WPF.ViewModels.LoginAndRegistration
{
    public class RegisterViewModel : UserCredentialForm
    {
        private const string EMAIL_EXISTS_MESSAGE = "Email already exists.";
        private const string INVALID_PASSWORD_MESSAGE = "Invalid password.";
        private const string USERNAME_EXISTS_MESSAGE = "Username already exists.";
        private const string CREATION_FAILED_MESSAGE = "Can't create account.";
        private const string NOT_MATCHING_PASSWORDS_MESSAGE = "Passwords do not match";

        private readonly IUserAccountService userAccountService;

        private string confirmPassword;
        private string firstName;
        private string lastName;

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

        /// <summary>
        /// Circular Dependency between the view models. Don't change this property as it will break the DI!
        /// </summary>
        public INavigationService<LoginViewModel> NavigationService { get; set; }

        public IAsyncCommand RegisterCommand { get; }
        public ICommand MoveBackCommand { get; }

        public RegisterViewModel(IUserAccountService userAccountService)
        {
            this.userAccountService = userAccountService;

            this.RegisterCommand = new RelayAsyncCommand(this.RegisterCommandAction, this.RegisterCommandPredicate, errorHandler: e =>
            {
                MessageDialogCreator.ShowErrorDialog(message: e.Message);
            });

            this.MoveBackCommand = new RelayCommand(_ => this.NavigationService.Navigate());
        }

        private async Task RegisterCommandAction()
        {
            var userAccountCreationResult = await this.userAccountService.CreateUserAccount(
                firstName: this.FirstName,
                lastName: this.LastName,
                userName: this.Username,
                email: this.Email,
                password: this.Password);

            if (userAccountCreationResult == UserAccountRegistrationResult.UserAccountCreated)
            {
                this.MoveBackCommand.Execute(null);
                return;
            }

            this.ErrorMessage = RegisterViewModel.GetCredentialsErrorMessage(userAccountCreationResult);
        }

        private static string GetCredentialsErrorMessage(UserAccountRegistrationResult errorType) => errorType switch
        {
            UserAccountRegistrationResult.EmailAlreadyExists => EMAIL_EXISTS_MESSAGE,
            UserAccountRegistrationResult.InvalidPassword => INVALID_PASSWORD_MESSAGE,
            UserAccountRegistrationResult.UserNameAlreadyExists => USERNAME_EXISTS_MESSAGE,
            UserAccountRegistrationResult.CanNotCreateUserAccount => CREATION_FAILED_MESSAGE,
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
                this.ErrorMessage = RegisterViewModel.NOT_MATCHING_PASSWORDS_MESSAGE;
            }
            // Hide the error message if it's already shown
            else if (this.ErrorMessage == RegisterViewModel.NOT_MATCHING_PASSWORDS_MESSAGE)
            {
                this.ErrorMessage = string.Empty;
            }

            return areEqualPasswords;
        }
    }
}
