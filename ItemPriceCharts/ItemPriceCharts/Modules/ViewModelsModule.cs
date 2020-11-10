﻿using Autofac;

using ItemPriceCharts.UI.WPF.ViewModels;
using ItemPriceCharts.UI.WPF.Views;
using ItemPriceCharts.UI.WPF.Views.UserControls;

namespace ItemPriceCharts.UI.WPF.Modules
{
    public class ViewModelsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ShopsView>();
            builder.RegisterType<ShopsViewModel>();
            builder.RegisterType<ViewItemsViewModel>();

            builder.RegisterType<CreateShopViewModel>();
            builder.RegisterType<CreateShopView>();

            builder.RegisterType<DeleteShopViewModel>();
            builder.RegisterType<DeleteShopView>();

            builder.RegisterType<CreateItemViewModel>();
            builder.RegisterType<CreateItemView>();

            builder.RegisterType<DeleteItemViewModel>();
            builder.RegisterType<DeleteItemView>();

            builder.RegisterType<ItemInformationViewModel>();
            builder.RegisterType<ItemInformationView>();
        }
    }
}
