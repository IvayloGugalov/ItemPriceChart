using System.Threading.Tasks;

using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Helpers;

namespace ItemPriceCharts.UI.WPF.ViewModels.LoginAndRegistration
{
    public class RegisterViewModel : UserCredentialForm
    {
        private string repeatPassword;
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

        public string RepeatPassword
        {
            get => this.repeatPassword;
            set => this.SetValue(ref this.repeatPassword, value);
        }

        public IAsyncCommand RegisterCommand { get; }

        public RegisterViewModel()
        {
            this.RegisterCommand = new RelayAsyncCommand(this.RegisterCommandAction, this.RegisterCommandPredicate, errorHandler: e =>
            {
                MessageDialogCreator.ShowErrorDialog(message: e.Message);
            });
        }

        private async Task RegisterCommandAction()
        {
            await Task.Delay(0);

        }

        private bool RegisterCommandPredicate()
        {
            return this.AreCredentialsFilled() &&
                   this.ArePasswordsEqual();
        }

        public override bool AreCredentialsFilled()
        {
            return !string.IsNullOrWhiteSpace(this.FirstName) &&
                   !string.IsNullOrWhiteSpace(this.LastName) &&
                   !string.IsNullOrWhiteSpace(this.RepeatPassword);
        }

        private bool ArePasswordsEqual()
        {
            var areEqual = this.Password == this.RepeatPassword;
            if (!areEqual)
            {
                this.ShowError();
            }

            return areEqual;
        }

        private void ShowError()
        {
            
        }
    }
}
