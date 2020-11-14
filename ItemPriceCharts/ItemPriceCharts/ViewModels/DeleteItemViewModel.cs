using System;
using System.Threading.Tasks;
using System.Windows.Input;

using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Helpers;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class DeleteItemViewModel : BindableViewModel
    {
        private readonly IItemService itemService;
        private string operationResult;

        public Item ItemToDelete { get; }
        public string OperationResult
        {
            get => this.operationResult;
            set => this.SetValue(ref this.operationResult, value);
        }

        public ICommand DeleteItemCommand { get; }

        public DeleteItemViewModel(ItemService itemService, Item item)
        {
            this.itemService = itemService;
            this.ItemToDelete = item;

            this.DeleteItemCommand = new RelayCommand(_ => this.DeleteItemAction());
        }

        private void DeleteItemAction()
        {
            try
            {
                Task.Run(() => this.itemService.DeleteItem(this.ItemToDelete));
            }
            catch (Exception e)
            {
                UIEvents.ShowMessageDialog(
                    new MessageDialogViewModel(
                        title: "Error",
                        description: e.Message,
                        buttonType: ButtonType.Close));
            }
        }
    }
}
