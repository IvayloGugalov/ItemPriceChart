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

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class ItemInformationViewModel : BindableViewModel
    {

        private readonly IItemPriceService itemPriceService;
        private readonly IItemService itemService;

        private SeriesCollection priceCollection;
        private LineSeries lineSeries;
        private List<string> labels;

        public ItemModel Item { get; }

        public Func<double, string> YFormatter { get; set; }

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


        public ItemInformationViewModel(ItemPriceService itemPriceService, ItemService itemService, ItemModel item)
        {
            this.itemPriceService = itemPriceService;
            this.itemService = itemService;
            this.Item = item;

            this.LoadItemPriceInformation();

            this.UpdatePriceCommand = new RelayCommand(_ => this.UpdatePriceAction());
        }

        private void LoadItemPriceInformation()
        {
            try
            {
                var priceInformation = Task.Run(() => this.itemPriceService.GetPricesForItem(this.Item.Id)).Result;

                var dateOfPrices = priceInformation.Select(p => p.PriceDate.ToShortDateString());
                var oldPrices = priceInformation.Select(p => p.Price);

                this.Labels = dateOfPrices.ToList();

                //var dateOfPrices = new DateTime[]
                //{
                //    new DateTime(2020, 5, 10),
                //    new DateTime(2020, 4, 10),
                //    new DateTime(2020, 3, 10),
                //    new DateTime(2020, 2, 10),
                //    new DateTime(2020, 1, 10),
                //    new DateTime(2019, 12, 10),
                //};

                //var oldPrices = new double[]
                //{
                //5.66,
                //6.33,
                //10.10,
                //9.94,
                //9.50,
                //10.50
                //};

                this.lineSeries = new LineSeries
                {
                    Title = "Price",
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
                throw e;
            }
        }

        private void UpdatePriceAction()
        {
            try
            {
                var itemPrice = Task.Run(() => this.itemService.UpdateItemPrice(this.Item)).Result;

                if (itemPrice != null)
                {
                    this.lineSeries.Values.Add(itemPrice.Price);
                    this.Labels.Add(itemPrice.PriceDate.ToShortDateString());

                    this.OnPropertyChanged(() => this.PriceCollection);
                    this.OnPropertyChanged(() => this.Labels);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
