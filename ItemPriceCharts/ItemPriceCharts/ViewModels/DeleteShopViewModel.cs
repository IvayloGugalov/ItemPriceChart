using System;
using System.Windows.Input;
using System.Threading.Tasks;

using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class DeleteShopViewModel : BindableViewModel
    {
        private readonly IOnlineShopService onlineShopService;
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

        public DeleteShopViewModel(OnlineShopService onlineShopService, OnlineShopModel selectedShop)
        {
            this.onlineShopService = onlineShopService;
            this.SelectedShop = selectedShop ?? throw new ArgumentNullException();

            this.DeleteShopCommand = new RelayCommand(_ => this.DeleteShopAction());
        }

        private async void DeleteShopAction()
        {
            try
            {
                await Task.Run(() => this.onlineShopService.DeleteShop(this.SelectedShop));

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