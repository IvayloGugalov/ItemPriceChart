using System;
using System.Windows.Input;
using System.Threading.Tasks;

using NLog;

using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class DeleteShopViewModel : BindableViewModel
    {
        private static readonly Logger logger = LogManager.GetLogger(nameof(DeleteShopViewModel));

        private readonly IOnlineShopService onlineShopService;
        private string operationResult = string.Empty;
        private bool isOperationRunning;

        public string OperationResult
        {
            get => this.operationResult;
            set => this.SetValue(ref this.operationResult, value);
        }

        public bool IsOperationRunning
        {
            get => this.isOperationRunning;
            set => this.SetValue(ref this.isOperationRunning, value);
        }

        public OnlineShop SelectedShop { get; }

        public ICommand DeleteShopCommand { get; }

        public DeleteShopViewModel(OnlineShopService onlineShopService, OnlineShop selectedShop)
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

                this.IsOperationRunning = true;
                this.OperationResult = $"Deleted {this.SelectedShop.Title} with id: {this.SelectedShop.Id}";
            }
            catch (Exception e)
            {
                logger.Info($"Can't delete {this.SelectedShop}.\t{e}");
                this.OperationResult = $"{e}. Failed to delete";
            }
            finally
            {
                this.IsOperationRunning = false;
            }
        }
    }
}