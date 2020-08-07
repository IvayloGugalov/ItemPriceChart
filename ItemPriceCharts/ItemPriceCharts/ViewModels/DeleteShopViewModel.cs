using System;
using System.Windows.Input;

using ItemPriceCharts.UI.WPF.Views;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.Services.Models;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class DeleteShopViewModel : BindableViewModel
    {
        private readonly IWebPageService webPageService;
        private OnlineShopModel selectedShop;
        private string operationResult = string.Empty;
        private bool isOperationFinished;

        public OnlineShopModel SelectedShop
        {
            get => this.selectedShop;
            set => this.SetValue(ref this.selectedShop, value);
        }

        public string OperationResult
        {
            get => this.operationResult;
            set => this.SetValue(ref this.operationResult, value);
        }

        public bool IsOperationFinished
        {
            get => this.isOperationFinished;
            set => this.SetValue(ref this.isOperationFinished, value);
        }

        public ObservableCollection<OnlineShopModel> AllShops { get; set; }

        public ICommand DeleteShopCommand { get; }

        public DeleteShopViewModel(WebPageService webPageService)
        {
            this.webPageService = webPageService;

            this.AllShops = ToObservableCollectionExtensions.ToObservableCollection(this.webPageService.RetrieveOnlineShops());
            this.SelectedShop = this.AllShops.First();

            this.DeleteShopCommand = new RelayCommand(_ => this.DeleteShopAction());

            var view = new DeleteShopView(this);
            view.Show();
        }

        private void DeleteShopAction()
        {
            try
            {
                this.IsOperationFinished = true;

                this.webPageService.DeleteShop(this.SelectedShop);
                this.OperationResult = $"Deleted {this.SelectedShop.Title} with id: {this.SelectedShop.Id}";

                this.AllShops.Remove(this.SelectedShop);
                //Events.ShopDeleted.Publish(this.SelectedShop.Title);
            }
            catch (Exception e)
            {
                this.OperationResult = $"{e}. Failed to delete";
            }
        }
    }
}