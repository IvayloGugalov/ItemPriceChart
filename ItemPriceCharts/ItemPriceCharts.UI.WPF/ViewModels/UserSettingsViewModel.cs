using System;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;

using NLog;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Events;
using ItemPriceCharts.UI.WPF.Extensions;
using ItemPriceCharts.UI.WPF.Services;
using ItemPriceCharts.UI.WPF.ViewModels.Base;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class UserSettingsViewModel : BaseViewModel
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(UserSettingsViewModel));

        private readonly IImageService imageService;
        public UserAccount UserAccount
        {
            get => this.userAccount;
            set => this.SetValue(ref this.userAccount, value);
        }
        private UserAccount userAccount;

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

        public bool IsUpdatingProfileImage
        {
            get => this.isUpdatingProfileImage;
            set => this.SetValue(ref this.isUpdatingProfileImage, value);
        }
        private bool isUpdatingProfileImage;

        public bool IsTwoFactorAuthEnabled => false;

        public BitmapImage UserProfileImage { get; private set; }

        public ICommand UpdateProfileImageCommand { get; }
        public ICommand UpdateEmailCommand { get; }
        public ICommand UpdatePasswordCommand { get; }
        public ICommand EnableTwoStepVerificationCommand { get; }
        public ICommand DisableTwoStepVerificationCommand { get; }
        public ICommand CloseAccountCommand { get; }

        public UserSettingsViewModel(IImageService imageService, UserAccount userAccount)
        {
            this.imageService = imageService;
            this.UserAccount = userAccount;

            this.UpdateProfileImageCommand = new RelayAsyncCommand(this.UpdateProfileImageAction);
            this.UpdateEmailCommand = new RelayCommand(_ =>
            {
                UiEvents.ShowUpdateEmailView.Raise(this.UserAccount);
                this.OnPropertyChanged(nameof(this.UserAccount));
            });
            this.UpdatePasswordCommand = new RelayCommand(_ => {});
            this.EnableTwoStepVerificationCommand = new RelayCommand(_ => { });
            this.DisableTwoStepVerificationCommand = new RelayCommand(_ => { });
            this.CloseAccountCommand = new RelayCommand(_ => {});
            
            UiEvents.UserAccountUpdated.Register(updatedAccount => this.UserAccount = updatedAccount);

            this.SetProfileImage().FireAndForgetSafeAsync(continueOnCapturedContext: false);
        }

        private async Task SetProfileImage()
        {
            string profileImagePath = null;
            var imageFound = await Task.Run(() => this.imageService.TryGetProfileImagePath(out profileImagePath));

            if (imageFound && !string.IsNullOrEmpty(profileImagePath))
            {
                this.UserProfileImage = new BitmapImage(new Uri(profileImagePath));
                this.OnPropertyChanged(nameof(this.UserProfileImage));
            }
        }

        private async Task UpdateProfileImageAction()
        {
            // TODO: Add PropertyObserver
            this.IsUpdatingProfileImage = true;

            try
            {
                var profileImagePath = string.Empty;
                var imageCreated = await Task.Run(() => this.imageService.TryCreateUserProfileImage(out profileImagePath));

                if (imageCreated)
                {
                    var profileImage = new BitmapImage(new Uri(profileImagePath));

                    this.UserProfileImage = profileImage;
                    this.OnPropertyChanged(nameof(this.UserProfileImage));

                    return;
                }

                // TODO: Show error message
            }
            catch (Exception e)
            {
                Logger.Error(e, "Could not update users' profile image.");
            }
            finally
            {
                this.IsUpdatingProfileImage = false;
            }
        }



    }
}
