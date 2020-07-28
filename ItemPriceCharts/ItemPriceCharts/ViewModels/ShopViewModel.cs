using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using ItemPriceCharts.Helpers;
using ItemPriceCharts.ViewModels;
using Services.Models;
using Services.Services;

namespace UI.WPF.ViewModels
{
    public class ShopViewModel : BindableViewModel
    {
        private readonly WebPageService webPageService;

        private string title;
        private bool isAddShopShown;
        private bool isChartShown;

        public bool IsAddURLEnabled { get; set; }

        public string Title
        {
            get => this.title;
            set => this.SetValue(ref this.title, value);
        }

        public bool IsAddShopShown
        {
            get => this.isAddShopShown;
            set => this.SetValue(ref this.isAddShopShown, value);
        }

        public bool IsChartShown
        {
            get => this.isChartShown;
            set => this.SetValue(ref isChartShown, value);
        }

        public ICommand ShowCreateShopCommand { get; }

        public ICommand ShowDeleteShopCommand { get; }

        public ICommand ShowAddItemCommand { get;  }

        public ShopViewModel(WebPageService webPageService)
        {
            this.webPageService = webPageService;

            this.ShowCreateShopCommand = new RelayCommand(_ => this.ShowCreateShopAction());
            this.ShowDeleteShopCommand = new RelayCommand(_ => this.ShowDeleteShopAction());
            this.ShowAddItemCommand = new RelayCommand(_ => this.ShowAddItemAction());
        }

        private void ShowCreateShopAction()
        {
            _ = new CreateShopViewModel(this.webPageService);
        }

        private void ShowDeleteShopAction()
        {
            _ = new DeleteShopViewModel(this.webPageService);
        }

        private void ShowAddItemAction()
        {
            _ = new CreateItemViewModel(this.webPageService);
        }
    }
}
