using ItemPriceCharts.UI.WPF.Helpers;

namespace ItemPriceCharts.UI.WPF.ViewModels.LoginAndRegistration
{
    public class UserCredentialForm : BindableViewModel
    {
        private string username;
        private string email;
        private string password;
        private string errorMessage;

        public string Username
        {
            get => this.username;
            set => this.SetValue(ref this.username, value);
        }

        public string Email
        {
            get => this.email;
            set => this.SetValue(ref this.email, value);
        }

        public string Password
        {
            get => this.password;
            set => this.SetValue(ref this.password, value);
        }

        public string ErrorMessage
        {
            get => this.errorMessage;
            set => this.SetValue(ref this.errorMessage, value);
        }

        public virtual bool AreCredentialsFilled()
        {
            return !string.IsNullOrWhiteSpace(this.Username) &&
                   !string.IsNullOrWhiteSpace(this.Email) &&
                   !string.IsNullOrWhiteSpace(this.Password);
        }

        public bool IsEmailValid => Validators.IsValidEmail(this.Email);
    }
}
