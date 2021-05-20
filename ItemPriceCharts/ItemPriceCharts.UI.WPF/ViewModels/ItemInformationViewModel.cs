using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LiveCharts;
using LiveCharts.Wpf;
using NLog;

using ItemPriceCharts.UI.WPF.CommandHelpers;
using ItemPriceCharts.UI.WPF.Helpers;
using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.Infrastructure.Services;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class ItemInformationViewModel : BindableViewModel
    {
        private static readonly Logger logger = LogManager.GetLogger(nameof(ItemInformationViewModel));

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
            private set => this.SetValue(ref this.isInProgress, value);
        }

        public List<string> Labels
        {
            get => this.labels;
            private set => this.SetValue(ref this.labels, value);
        }

        public SeriesCollection PriceCollection
        {
            get => this.priceCollection;
            private set => this.SetValue(ref this.priceCollection, value);
        }

        public LineSeries LineSeries
        {
            get => this.lineSeries;
            private set => lineSeries = value;
        }

        public IAsyncCommand UpdatePriceCommand { get; }

        public ItemInformationViewModel(IItemService itemService, Item item)
        {
            this.itemService = itemService;
            this.Item = item;

            this.UpdatePriceCommand = new RelayAsyncCommand(this.UpdatePriceAction, this.UpdatePricePredicate, errorHandler: e =>
            {
                logger.Error($"Couldn't update item price: {e}");
                MessageDialogCreator.ShowErrorDialog(message: $"Could not update price for {this.Item.Title}");
            });

            this.LoadItemPriceInformationAsync();
        }

        private void LoadItemPriceInformationAsync()
        {
            var priceInformation = this.Item.PricesForItem;
            var dateOfPrices = priceInformation.Select(p => p.PriceDateRetrieved.ToShortDateString());
            var oldPrices = priceInformation.Select(p => p.Price);

            //Test Data
            //this.CreateTestData(out var dateOfPrices, out var oldPrices);

            this.Labels = dateOfPrices.ToList();

            this.LineSeries = new LineSeries
            {
                Title = "Price",
                Stroke = System.Windows.Media.Brushes.White,
                Values = new ChartValues<double>(oldPrices)
            };

            this.PriceCollection = new SeriesCollection
            {
                this.LineSeries
            };

            this.YFormatter = value => value.ToString("C");
        }

        private async Task UpdatePriceAction()
        {
            this.IsInProgress = true;

            var updatedItemPrice = await this.itemService.UpdateItemPrice(this.Item);

            if (updatedItemPrice != null)
            {
                this.LineSeries.Values.Add(updatedItemPrice.Price);
                this.Labels.Add(updatedItemPrice.PriceDateRetrieved.ToShortDateString());
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
