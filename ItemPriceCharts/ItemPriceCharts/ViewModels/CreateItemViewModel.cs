using System;
using System.Threading.Tasks;
using System.Windows.Input;

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

        public bool IsAddingItemFinished { get; private set; }

        public OnlineShop SelectedShop { get; }

        public ICommand AddItemCommand { get; }

        public CreateItemViewModel(ItemService itemService, OnlineShop selectedShop)
        {
            this.itemService = itemService;
            this.SelectedShop = selectedShop ?? throw new ArgumentNullException();

            this.AddItemCommand = new RelayCommand<object>(this.AddItemAction, this.AddItemPredicate);
        }

        private void AddItemAction(object param)
        {
            try
            {
                this.IsAddingItemFinished = true;
                this.OnPropertyChanged(nameof(this.IsAddingItemFinished));

                Task.Run(() => this.itemService.CreateItem(this.NewItemURL, this.SelectedShop, this.SelectedItemType))
                    .ContinueWith((_) =>
                    {
                        this.IsAddingItemFinished = false;
                        this.OnPropertyChanged(nameof(this.IsAddingItemFinished));
                    });
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
        }

        private bool AddItemPredicate()
        {
            return ValidateURL.IsValidAddress(this.NewItemURL) && 
                this.NewItemURL.ToLower().Contains(this.SelectedShop.Title.ToLower());
        }
    }
}
