using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.ViewModels.Base;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class UserSettingsViewModel : BaseViewModel
    {
        public UserAccount UserAccount { get; }

        public string CurrentPassword
        {
            get => this.currentPassword;
            set => this.SetValue(ref this.currentPassword, value);
        }
        private string currentPassword;

        public string NewPassword
        {
            get => this.newPassword;
            set => this.SetValue(ref this.newPassword, value);
        }
        private string newPassword;

        public string RepeatNewPassword
        {
            get => this.repeatNewPassword;
            set => this.SetValue(ref this.repeatNewPassword, value);
        }
        private string repeatNewPassword;

        public bool IsTwoFactorAuthEnabled => false;

        public ICommand ChooseImageCommand { get; }
        public ICommand UpdateEmailCommand { get; }
        public ICommand UpdatePasswordCommand { get; }
        public ICommand EnableTwoStepVerificationCommand { get; }
        public ICommand DisableTwoStepVerificationCommand { get; }
        public ICommand CloseAccountCommand { get; }

        public UserSettingsViewModel(UserAccount userAccount)
        {
            this.UserAccount = userAccount;




            this.ChooseImageCommand = new RelayCommand(_ => {});
            this.UpdateEmailCommand = new RelayCommand(_ => {});
            this.UpdatePasswordCommand = new RelayCommand(_ => {});
            this.EnableTwoStepVerificationCommand = new RelayCommand(_ => { });
            this.DisableTwoStepVerificationCommand = new RelayCommand(_ => { });
            this.CloseAccountCommand = new RelayCommand(_ => {});
        }
    }
}
