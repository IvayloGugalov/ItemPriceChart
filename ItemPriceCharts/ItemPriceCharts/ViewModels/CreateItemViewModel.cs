using System;
using System.Threading.Tasks;

using NLog;

using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Helpers;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class CreateItemViewModel : BindableViewModel
    {
        private static readonly Logger logger = LogManager.GetLogger(nameof(CreateItemViewModel));

        private readonly IItemService itemService;
        private ItemType selectedItemType;
        private string newItemURL;

        public string NewItemURL
        {
            get => this.newItemURL;
            set => this.SetValue(ref this.newItemURL, value);
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

            this.AddItemCommand = new RelayAsyncCommand(this.AddItemAction, this.AddItemPredicate);
        }

        private async Task AddItemAction()
        {
            this.IsInProgress = true;
            this.OnPropertyChanged(nameof(this.IsInProgress));

            await Task.Run(() =>
            {
                try
                {
                    this.itemService.CreateItem(this.NewItemURL, this.SelectedShop, this.SelectedItemType);
                }
                catch (Exception e)
                {
                    logger.Info($"Failed to create {this.NewItemURL} due to: {e}");
                    UIEvents.ShowMessageDialog(
                            new MessageDialogViewModel(
                                title: "Error",
                                description: e.Message,
                                buttonType: ButtonType.Close));
                }
            })
            .ContinueWith(_ =>
            {
                this.IsInProgress = false;
                this.OnPropertyChanged(nameof(this.IsInProgress));
            });
        }

        private bool AddItemPredicate()
        {
            return ValidateURL.IsValidAddress(this.NewItemURL) && 
                this.NewItemURL.ToLower().Contains(this.SelectedShop.Title.ToLower());
        }
    }
}
