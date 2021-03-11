using ItemPriceCharts.UI.WPF.CommandHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class LoginViewModel : BindableViewModel
    {
        private string username;
        private string password;

        public string Username
        {
            get => this.username;
            set => this.SetValue(ref this.username, value);
        }
        
        public string Password
        {
            get => this.password;
            set => this.SetValue(ref this.password, value);
        }

        public IAsyncCommand LoginCommand { get; }

        public LoginViewModel()
        {
            this.LoginCommand = new RelayAsyncCommand(this.LoginCommandAction, this.LoginCommandPredicate, errorHandler: e =>
            {
            });
        }

        private bool LoginCommandPredicate()
        {
            throw new NotImplementedException();
        }

        private Task LoginCommandAction()
        {
            throw new NotImplementedException();
        }
    }
}
