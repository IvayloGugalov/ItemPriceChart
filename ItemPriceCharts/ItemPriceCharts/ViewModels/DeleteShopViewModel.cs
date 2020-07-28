using ItemPriceCharts.Helpers;
using Services.Services;
using System;
using System.Windows.Input;
using UI.WPF.Views;

namespace UI.WPF.ViewModels
{
    public class DeleteShopViewModel : BindableViewModel
    {
        private readonly WebPageService webPageService;
        private string shopId;
        private string operationResult = string.Empty;
        private bool isOperationFinished;

        public string ShopId
        {
            get => this.shopId;
            set => this.SetValue(ref this.shopId, value);
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

        public ICommand DeleteShopCommand { get; }

        public DeleteShopViewModel(WebPageService webPageService)
        {
            this.webPageService = webPageService;

            this.DeleteShopCommand = new RelayCommand(_ => this.DeleteShopAction());

            var view = new DeleteShopView(this);
            view.Show();
        }

        private void DeleteShopAction()
        {
            try
            {
                this.IsOperationFinished = true;
                if (Int32.TryParse(shopId, out var id))
                {
                    this.webPageService.DeleteShop(id);
                    this.OperationResult = $"Successfully delete shop with id: {id}.";
                }
            }
            catch (Exception e)
            {
                this.OperationResult = $"{e}. Failed to delete";
            }
            finally
            {

            }
        }
    }
}