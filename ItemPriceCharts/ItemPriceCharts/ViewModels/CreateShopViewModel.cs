using System;
using System.Windows.Input;

using ItemPriceCharts.Helpers;
using Services.Services;
using UI.WPF.Views;

namespace UI.WPF.ViewModels
{
    public class CreateShopViewModel : BindableViewModel
    {
        private readonly CreateShopView view;
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

            this.view = new CreateShopView(this);
            view.ShowDialog();
        }

        private void AddShopAction()
        {
            try
            {
                if (this.NewShopURL != string.Empty && this.IsCorrctURL())
                {
                    this.webPageService.CreateShop(this.NewShopURL, this.NewShopTitle);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                this.view.Close();
            }
        }

        private bool IsCorrctURL()
        {
            return this.NewShopURL.EndsWith(".com") || this.NewShopURL.EndsWith(@".bg/");
        }
    }
}
