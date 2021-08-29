using System;
using System.Collections.Generic;
using System.Windows.Threading;

using Autofac;

using ItemPriceCharts.Infrastructure.Services;
using ItemPriceCharts.UI.WPF.Factories;
using ItemPriceCharts.UI.WPF.Services;
using ItemPriceCharts.UI.WPF.ViewModels.LoginAndRegistration;

namespace ItemPriceCharts.UI.WPF.Modules
{
    public class MappedTypeModules : Module
    {
        private readonly Dispatcher dispatcher;

        public MappedTypeModules(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DispatcherWrapper>()
                .As<IDispatcherWrapper>()
                .SingleInstance()
                .WithParameter(new TypedParameter(typeof(Dispatcher), this.dispatcher));

            builder.RegisterType<HtmlWebWrapper>()
                .As<IHtmlWebWrapper>()
                .InstancePerDependency()
                .WithParameter(new TypedParameter(typeof(HtmlAgilityPack.HtmlWeb), new HtmlAgilityPack.HtmlWeb()));

            builder.RegisterType<NavigationService<LoginViewModel>>()
                .As<INavigationService<LoginViewModel>>()
                .SingleInstance();
            builder.RegisterType<NavigationService<RegisterViewModel>>()
                .As<INavigationService<RegisterViewModel>>()
                .SingleInstance();

            var mappedTypes = new Dictionary<Type, Type>
            {
                {typeof(ILogOutService), typeof(LogOutService)},
                {typeof(IItemService), typeof(ItemService)},
                {typeof(IOnlineShopService), typeof(OnlineShopService)},
                {typeof(IUserAccountService), typeof(UserAccountService)},
                {typeof(IItemDataRetrieveService), typeof(ItemDataRetrieveService)}
            };

            foreach (var (type, value) in mappedTypes)
            {
                builder.RegisterType(value)
                    .As(type)
                    .InstancePerLifetimeScope();
            }
        }
    }
}
