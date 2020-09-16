using ItemPriceCharts.Services.Models;
using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.CommandHelpers;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class ItemInformationViewModel : BindableViewModel
    {

        private readonly IItemPriceService itemPriceService;
        private readonly IItemService itemService;

        public Func<double, string> YFormatter { get; set; }

        public List<string> Labels { get; set; }

        public ItemModel Item { get; }

        public SeriesCollection PriceCollection { get; set; }

        public ICommand UpdatePriceCommand { get; }


        public ItemInformationViewModel(ItemPriceService itemPriceService, ItemService itemService, ItemModel item)
        {
            this.itemPriceService = itemPriceService;
            this.itemService = itemService;
            this.Item = item;

            this.LoadItemPriceInformation();

            this.UpdatePriceCommand = new RelayCommand(_ => this.UpdatePriceAction());

            //this.Labels = Enumerable.Range(1, DateTime.Today.Month - 1)
            //                .Select(m => new DateTime(DateTime.Today.Year, m, 1).ToString("MMMM"))
            //                .ToArray();
        }

        private void LoadItemPriceInformation()
        {
            try
            {
                var priceInformation = Task.Run(() => this.itemPriceService.GetPricesForItem(this.Item.Id)).Result;

                //var dateOfPrices = priceInformation.Select(p => p.PriceDate.ToString("MM/dd/yyyy"));
                //var oldPrices = priceInformation.Select(p => p.Price);


                var dateOfPrices = new DateTime[]
                {
                    new DateTime(2020, 5, 10),
                    new DateTime(2020, 4, 10),
                    new DateTime(2020, 3, 10),
                    new DateTime(2020, 2, 10),
                    new DateTime(2020, 1, 10),
                    new DateTime(2019, 12, 10),
                };

                this.Labels = dateOfPrices.Select(d => d.ToShortDateString()).ToList();
                
                var oldPrices = new double[]
                {
                5.66,
                6.33,
                10.10,
                9.94,
                9.50,
                10.50
                };

                this.PriceCollection = new SeriesCollection
                {
                    new LineSeries
                    {
                        Title = "Price",
                        Values = new ChartValues<double>(oldPrices)
                    }
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
                //check if shop exists
                var itemPrice =  Task.Run(() => this.itemService.UpdateItemPrice(this.Item)).Result;

                this.Labels.Add(itemPrice.PriceDate.ToShortDateString());

                this.OnPropertyChanged(() => this.Labels);
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
