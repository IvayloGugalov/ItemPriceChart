using System.Threading.Tasks;
using ItemPriceCharts.Domain.Entities;
using NLog;

using ItemPriceCharts.Infrastructure.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Helpers;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class CreateShopViewModel : BindableViewModel
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(CreateShopViewModel));

        private readonly IOnlineShopService onlineShopService;
        private readonly UserAccount userAccount;

        private string newShopUrl;
        private string newShopTitle;

        public string NewShopTitle
        {
            get => this.newShopTitle;
            set => this.SetValue(ref this.newShopTitle, value);
        }

        public string NewShopUrl
        {
            get => this.newShopUrl;
            set => this.SetValue(ref this.newShopUrl, value);
        }

        public IAsyncCommand AddShopCommand { get; }

        public CreateShopViewModel(IOnlineShopService onlineShopService, UserAccount userAccount)
        {
            this.onlineShopService = onlineShopService;
            this.userAccount = userAccount;
            this.AddShopCommand = new RelayAsyncCommand(this.AddShopAction, this.AddShopPredicate, errorHandler: e =>
            {
                Logger.Error($"Failed to create new shop: {e}");
                MessageDialogCreator.ShowErrorDialog(message: $"Failed to create new shop with url: {this.NewShopUrl}");
            });
        }

        private async Task AddShopAction()
        {
            await this.onlineShopService.CreateShop(this.userAccount, this.NewShopUrl, this.NewShopTitle);
        }

        private bool AddShopPredicate() => Validators.IsValidAddress(this.NewShopUrl);
    }
}
