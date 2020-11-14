using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

using LiveCharts;
using LiveCharts.Wpf;

using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Helpers;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class ItemInformationViewModel : BindableViewModel
    {

        private readonly IItemPriceService itemPriceService;
        private readonly IItemService itemService;

        private SeriesCollection priceCollection;
        private LineSeries lineSeries;
        private List<string> labels;
        private bool isInProgress;

        public Item Item { get; }

        public Func<double, string> YFormatter { get; private set; }

        public bool IsInProgress
        {
            get => this.isInProgress;
            set => this.SetValue(ref this.isInProgress, value);
        }

        public List<string> Labels
        {
            get => this.labels;
            set => this.SetValue(ref this.labels, value);
        }

        public SeriesCollection PriceCollection
        {
            get => this.priceCollection;
            set => this.SetValue(ref this.priceCollection, value);
        }

        public ICommand UpdatePriceCommand { get; }

        public ItemInformationViewModel(ItemPriceService itemPriceService, ItemService itemService, Item item)
        {
            this.itemPriceService = itemPriceService;
            this.itemService = itemService;
            this.Item = item;

            this.LoadItemPriceInformation();

            this.UpdatePriceCommand = new RelayCommand<object>(this.UpdatePriceAction, this.UpdatePricePredicate);
        }

        private async void LoadItemPriceInformation()
        {
            try
            {
                var priceInformation = await Task.Run(() => this.itemPriceService.GetPricesForItem(this.Item.Id));

                var dateOfPrices = priceInformation.Select(p => p.PriceDate.ToShortDateString());
                var oldPrices = priceInformation.Select(p => p.Price);

                //Test Data
                //this.CreateTestData(out var dateOfPrices, out var oldPrices);

                this.Labels = dateOfPrices.ToList();

                this.lineSeries = new LineSeries
                {
                    Title = "Price",
                    Stroke = System.Windows.Media.Brushes.White,
                    Values = new ChartValues<double>(oldPrices)
                };

                this.PriceCollection = new SeriesCollection
                {
                    this.lineSeries
                };

                this.YFormatter = value => value.ToString("C");
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

        private async void UpdatePriceAction(object obj)
        {
            try
            {
                this.IsInProgress = true;

                ItemPrice updatedItemPrice = null;
                var isUpdated = await Task.Run(() => this.itemService.UpdateItemPrice(this.Item, out updatedItemPrice));

                if (isUpdated)
                {
                    this.lineSeries.Values.Add(updatedItemPrice.Price);
                    this.Labels.Add(updatedItemPrice.PriceDate.ToShortDateString());

                    this.OnPropertyChanged(() => this.PriceCollection);
                    this.OnPropertyChanged(() => this.Labels);
                }
                else
                {
                    UIEvents.ShowMessageDialog(
                        new MessageDialogViewModel(
                            title: "Item Price Chart",
                            description: "The price of the item hasn't been changed.",
                            buttonType: ButtonType.Close));
                }
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

        private bool UpdatePricePredicate()
        {
            return !this.IsInProgress;
        }

        private void CreateTestData(out List<string> dateOfPrices, out double[] oldPrices)
        {
            dateOfPrices = new List<string>
            {
                new DateTime(2020, 5, 10).ToShortDateString(),
                new DateTime(2020, 4, 10).ToShortDateString(),
                new DateTime(2020, 3, 10).ToShortDateString(),
                new DateTime(2020, 2, 10).ToShortDateString(),
                new DateTime(2020, 1, 10).ToShortDateString(),
                new DateTime(2019, 12, 10).ToShortDateString(),
            }.ToList();

            oldPrices = new double[]
            {
                5.66,
                6.33,
                10.10,
                9.94,
                9.50,
                10.50
            };
        }
    }
}
