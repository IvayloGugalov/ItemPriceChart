﻿using System.Threading.Tasks;

using NLog;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.Infrastructure.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.UI.WPF.ViewModels.Base;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class DeleteItemViewModel : BaseViewModel
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(DeleteItemViewModel));

        private readonly IItemService itemService;

        public Item ItemToDelete { get; }

        public IAsyncCommand DeleteItemCommand { get; }

        public DeleteItemViewModel(IItemService itemService, Item item)
        {
            this.itemService = itemService;
            this.ItemToDelete = item;

            this.DeleteItemCommand = new RelayAsyncCommand(this.DeleteItemAction, errorHandler: e =>
            {
                Logger.Error($"Can't delete item {this.ItemToDelete}.\n{e}");
                MessageDialogCreator.ShowErrorDialog(message: $"Couldn't delete item {this.ItemToDelete.Title}");
            });
        }

        private async Task DeleteItemAction()
        {
            await this.itemService.DeleteItem(this.ItemToDelete);
        }
    }
}
