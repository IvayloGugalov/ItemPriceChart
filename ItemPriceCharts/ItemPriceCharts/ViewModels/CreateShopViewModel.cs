using System;
using System.Threading.Tasks;
using System.Windows.Input;

using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Helpers;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class CreateShopViewModel : BindableViewModel
    {
        private readonly IOnlineShopService onlineShopService;
        private string newShopURL;
        private string newShopTitle;

        public string NewShopTitle
        {
            get => this.newShopTitle;
            set => this.SetValue(ref this.newShopTitle, value);
        }

        public string NewShopURL
        {
            get => this.newShopURL;
            set => this.SetValue(ref this.newShopURL, value);
        }

        public ICommand AddShopCommand { get; }

        public CreateShopViewModel(OnlineShopService onlineShopService)
        {
            this.onlineShopService = onlineShopService;

            this.AddShopCommand = new RelayCommand(_ => this.AddShopAction());
        }

        private async void AddShopAction()
        {
            try
            {
                if (ValidateURL.IsValidAddress(this.NewShopURL))
                {
                    await Task.Run(() => this.onlineShopService.CreateShop(this.NewShopURL, this.NewShopTitle));
                }
            }
            catch (Exception e)
            {
                UIEvents.ShowMessageDialog(
                    new MessageDialogViewModel(
                        title: "Error",
                        description: e.Message,
                        buttonType: ButtonType.Close));
            }
        }
    }
}
