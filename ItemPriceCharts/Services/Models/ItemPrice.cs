﻿using System;

using System.ComponentModel.DataAnnotations;

namespace ItemPriceCharts.Services.Models
{
    public sealed class ItemPrice
    {
        [Key]
        public int Id { get; set; }
        public DateTime PriceDate { get; set; }
        public double CurrentPrice { get; set; }

        public int ItemId { get; set; }

        public ItemPrice()
        {
        }

        public ItemPrice(int id, DateTime priceDate, double currentPrice, int itemId)
        {
            this.Id = id;
            this.PriceDate = priceDate;
            this.CurrentPrice = currentPrice;
            this.ItemId = itemId;
        }
    }
}
