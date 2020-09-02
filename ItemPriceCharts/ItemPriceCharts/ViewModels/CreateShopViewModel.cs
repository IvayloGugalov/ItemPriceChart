using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Helpers;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class CreateShopViewModel : BindableViewModel
    {
        private readonly WebPageService webPageService;
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

        public CreateShopViewModel(WebPageService webPageService)
        {
            this.webPageService = webPageService;

            this.AddShopCommand = new RelayCommand(_ => this.AddShopAction());
        }

        private void AddShopAction()
        {
            try
            {
                if (ValidateURL.IsValidAddress(this.NewShopURL))
                {
                    Task.Run(() => this.webPageService.CreateShop(this.NewShopURL, this.NewShopTitle));
                }
            }
            catch (Exception e)
            {
                MessageBox.Show($"We had an error: {e.Message}");
            }
        }
    }
}
