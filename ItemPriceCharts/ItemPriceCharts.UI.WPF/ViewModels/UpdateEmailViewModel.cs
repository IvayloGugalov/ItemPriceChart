using System;
using System.Threading.Tasks;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.Infrastructure.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Events;
using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.UI.WPF.ViewModels.Base;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class UpdateEmailViewModel : BaseViewModel
    {
        private readonly IUserAccountService userAccountService;
        private readonly UserAccount userAccount;

        public string NewEmailValue
        {
            get => this.newEmailValue;
            set
            {
                if (!string.IsNullOrEmpty(this.NewEmailErrorText))
                {
                    this.NewEmailErrorText = null;
                }
                this.UpdateEmailCommand.RaiseCanExecuteChanged();
                this.SetValue(ref this.newEmailValue, value);
            }
        }
        private string newEmailValue;
        
        public string NewEmailErrorText
        {
            get => this.newEmailErrorText;
            private set => this.SetValue(ref this.newEmailErrorText, value);
        }
        private string newEmailErrorText;

        public bool EmailUpdateSucceeded { get; set; }

        public RelayAsyncCommand UpdateEmailCommand { get; }

        public UpdateEmailViewModel(IUserAccountService userAccountService, UserAccount userAccount)
        {
            this.userAccountService = userAccountService;
            this.userAccount = userAccount ?? throw new ArgumentNullException(nameof(userAccount));

            this.UpdateEmailCommand = new RelayAsyncCommand(this.UpdateEmailAction, this.UpdateEmailPredicate);
        }

        private async Task UpdateEmailAction()
        {
            if (this.NewEmailValue == this.userAccount.Email.Value)
            {
                this.NewEmailErrorText = "New email can't be the same as the old email.";
                return;
            }

            if (!Validators.IsValidEmail(this.NewEmailValue))
            {
                this.NewEmailErrorText = "Email is not in valid format.";
                return;
            }

            await this.userAccountService.UpdateUserAccountEmail(this.userAccount.Id, this.newEmailValue);
            await this.userAccountService.WriteUserCredentials(this.userAccount);
            UiEvents.UserAccountUpdated.Raise(this.userAccount);
            this.EmailUpdateSucceeded = true;
            this.OnPropertyChanged(nameof(this.EmailUpdateSucceeded));
        }

        private bool UpdateEmailPredicate()
        {
            return string.IsNullOrEmpty(this.NewEmailErrorText) && !string.IsNullOrEmpty(this.NewEmailValue);
        }
    }
}
