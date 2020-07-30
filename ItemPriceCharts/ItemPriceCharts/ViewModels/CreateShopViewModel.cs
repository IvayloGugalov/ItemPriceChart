using System;
using System.Windows.Input;

using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.UI.WPF.Views;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class CreateShopViewModel : BindableViewModel
    {
        private const string SHOP_ADDED_EVENT_NAME = "NewShopAdded";

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

        public event PublishAndSubscribeEventHandler<object> UpdateListViewHandler;

        public CreateShopViewModel(WebPageService webPageService)
        {
            this.webPageService = webPageService;

            this.AddShopCommand = new RelayCommand(_ => this.AddShopAction());

            PublishSubscribe<object>.AddEvent(CreateShopViewModel.SHOP_ADDED_EVENT_NAME, this.UpdateListViewHandler);

            this.view = new CreateShopView(this);
            view.ShowDialog();
        }

        private void AddShopAction()
        {
            try
            {
                if (this.NewShopURL != string.Empty && this.IsCorrctURL())
                {
                    if (this.webPageService.CreateShop(this.NewShopURL, this.NewShopTitle))
                    {
                        //PublishSubscribe<object>.Publish(CreateShopViewModel.SHOP_ADDED_EVENT_NAME, this, new PublishAndSubscribeEventArgs<object>(this.NewShopTitle));
                        NewEvents.NewShopAddedPub.Publish(this.NewShopTitle);
                    }
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
