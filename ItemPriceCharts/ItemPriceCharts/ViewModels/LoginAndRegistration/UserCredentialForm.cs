using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemPriceCharts.UI.WPF.ViewModels.LoginAndRegistration
{
    public class UserCredentialForm : BindableViewModel
    {
        private string username;
        private string email;
        private string password;

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

        public virtual bool AreCredentialsFilled()
        {
            return !string.IsNullOrWhiteSpace(this.Username) &&
                   !string.IsNullOrWhiteSpace(this.Email) &&
                   !string.IsNullOrWhiteSpace(this.Password);
        }
    }
}
