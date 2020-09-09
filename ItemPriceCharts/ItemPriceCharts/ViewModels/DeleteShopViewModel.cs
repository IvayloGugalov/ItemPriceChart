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
using System.Threading.Tasks;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class DeleteShopViewModel : BindableViewModel
    {
        private readonly IWebPageService webPageService;
        private string operationResult = string.Empty;
        private bool isOperationFinished;

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

        public OnlineShopModel SelectedShop { get; }

        public ICommand DeleteShopCommand { get; }

        public DeleteShopViewModel(WebPageService webPageService, OnlineShopModel selectedShop)
        {
            this.webPageService = webPageService;
            this.SelectedShop = selectedShop ?? throw new ArgumentNullException();

            this.DeleteShopCommand = new RelayCommand(_ => this.DeleteShopAction());
        }

        private async void DeleteShopAction()
        {
            try
            {
                await Task.Run(() => this.webPageService.DeleteShop(this.SelectedShop));

                this.IsOperationFinished = true;
                this.OperationResult = $"Deleted {this.SelectedShop.Title} with id: {this.SelectedShop.Id}";

                this.OnPropertyChanged(() => this.IsOperationFinished);
                this.OnPropertyChanged(() => this.OperationResult);
            }
            catch (Exception e)
            {
                this.OperationResult = $"{e}. Failed to delete";
            }
        }
    }
}