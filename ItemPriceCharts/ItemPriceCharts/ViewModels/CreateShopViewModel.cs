using System;
using System.Threading.Tasks;
using System.Windows.Input;

using NLog;

using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Helpers;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class CreateShopViewModel : BindableViewModel
    {
        private static readonly Logger logger = LogManager.GetLogger(nameof(CreateShopViewModel));

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

        public IAsyncCommand AddShopCommand { get; }

        public CreateShopViewModel(IOnlineShopService onlineShopService)
        {
            this.onlineShopService = onlineShopService;

            this.AddShopCommand = new RelayAsyncCommand(this.AddShopAction, this.AddShopPredicate, errorHandler: e =>
            {
                logger.Error($"Failed to create new shop: {e}");
                MessageDialogCreator.ShowErrorDialog(message: $"Failed to create new shop with url: {this.NewShopURL}");
            });
        }

        private async Task AddShopAction()
        {
            await this.onlineShopService.CreateShop(this.NewShopURL, this.NewShopTitle);
        }

        private bool AddShopPredicate()
        {
            return Validators.IsValidAddress(this.NewShopURL);
        }
    }
}
