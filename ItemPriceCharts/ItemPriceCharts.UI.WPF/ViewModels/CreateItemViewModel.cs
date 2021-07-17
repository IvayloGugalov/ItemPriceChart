using System;
using System.Threading.Tasks;

using NLog;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.Domain.Enums;
using ItemPriceCharts.Infrastructure.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.UI.WPF.ViewModels.Base;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class CreateItemViewModel : BaseViewModel
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(CreateItemViewModel));

        private readonly IItemService itemService;
        private ItemType selectedItemType;
        private string newItemUrl;

        public string NewItemUrl
        {
            get => this.newItemUrl;
            set => this.SetValue(ref this.newItemUrl, value);
        }

        public ItemType SelectedItemType
        {
            get => this.selectedItemType;
            set => this.SetValue(ref this.selectedItemType, value);
        }

        public bool IsInProgress { get; private set; }

        public OnlineShop SelectedShop { get; }

        public IAsyncCommand AddItemCommand { get; }

        public CreateItemViewModel(IItemService itemService, OnlineShop selectedShop)
        {
            this.itemService = itemService;
            this.SelectedShop = selectedShop ?? throw new ArgumentNullException(nameof(selectedShop));

            this.AddItemCommand = new RelayAsyncCommand(this.AddItemAction, this.AddItemPredicate, errorHandler: e =>
            {
                Logger.Error($"Failed to create new item: {e}");
                MessageDialogCreator.ShowErrorDialog(message: $"Failed to create new item with url: {this.NewItemUrl}");
            });
        }

        private async Task AddItemAction()
        {
            this.IsInProgress = true;
            this.OnPropertyChanged(nameof(this.IsInProgress));

            await Task.Run(() => this.itemService.AddItemToShop(this.NewItemUrl, this.SelectedShop, this.SelectedItemType));

            this.IsInProgress = false;
            this.OnPropertyChanged(nameof(this.IsInProgress));
        }

        private bool AddItemPredicate() =>
            Validators.IsValidAddress(this.NewItemUrl) && 
            this.NewItemUrl.ToLower().Contains(this.SelectedShop.Title.ToLower());
    }
}
