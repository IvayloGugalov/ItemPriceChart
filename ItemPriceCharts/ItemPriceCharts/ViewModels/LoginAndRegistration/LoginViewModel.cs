using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

using ItemPriceCharts.Services.Helpers;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Helpers;

namespace ItemPriceCharts.UI.WPF.ViewModels.LoginAndRegistration
{
    public class LoginViewModel : UserCredentialForm
    {
        private bool rememberMeChecked;

        public bool RememberMeChecked
        {
            get => this.rememberMeChecked;
            set => this.SetValue(ref this.rememberMeChecked, value);
        }

        public bool ShowRegisterView { get; private set; }

        public IAsyncCommand LoginCommand { get; }
        public ICommand ChangeWindowViewCommand { get; }

        public LoginViewModel()
        {
            this.LoginCommand = new RelayAsyncCommand(this.LoginCommandAction, this.LoginCommandPredicate, errorHandler: e =>
            {
                MessageDialogCreator.ShowErrorDialog(message: e.Message);
            });

            this.ChangeWindowViewCommand = new RelayCommand(this.ChangeWindowViewAction);
        }

        private async Task LoginCommandAction()
        {
            await Task.Delay(0);
            //this.A(Services.Constants.Paths.XML_FILE_PATH);
            this.B(Services.Constants.Paths.XML_FILE_PATH);
        }

        // Add correct email checker
        private bool LoginCommandPredicate()
        {
            return this.AreCredentialsFilled();
        }

        private void ChangeWindowViewAction(object param)
        {
            this.ShowRegisterView = true;
            this.OnPropertyChanged(nameof(this.ShowRegisterView));
        }

        public void A(string filePath)
        {
            var writer = XmlHelper.CreateWriter(filePath);

            using (writer.WriteElementBody(nameof(Services.Models.User)))
            {
                writer.WriteTo(nameof(this.Username), this.Username);
                writer.WriteTo(nameof(this.Email), this.Email);
            }
        }

        public void B(string filePath)
        {
            var reader = XmlHelper.CreateReader(filePath);
            reader.Read();

            reader.Read(new Dictionary<string, Action>()
            {
                { nameof(this.Username), () => this.Username = reader.ReadElementContentAsString()},
                { nameof(this.Email), () => this.Email = reader.ReadElementContentAsString()}
            });
        }
    }
}
