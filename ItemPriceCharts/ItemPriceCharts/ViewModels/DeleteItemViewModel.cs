using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class DeleteItemViewModel : BindableViewModel
    {
        private readonly IWebPageService webPageService;
        private string operationResult;

        public ItemModel ItemToDelete { get; }
        public string OperationResult
        {
            get => this.operationResult;
            set => this.SetValue(ref this.operationResult, value);
        }

        ICommand DeleteItemCommand { get; }

        public DeleteItemViewModel(WebPageService webPageService, ItemModel item)
        {
            this.webPageService = webPageService;
            this.ItemToDelete = item;

            this.DeleteItemCommand = new RelayCommand(_ => this.DeleteItemAction());
        }

        private void DeleteItemAction()
        {
            try
            {
                Task.Run(() => this.webPageService.DeleteItem(this.ItemToDelete));
            }
            catch (Exception e)
            {
                MessageBox.Show($"We had an error: {e.Message}");
            }
        }
    }
}
