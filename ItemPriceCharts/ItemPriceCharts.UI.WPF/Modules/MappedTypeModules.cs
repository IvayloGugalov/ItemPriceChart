using System;
using System.Collections.Generic;

using Autofac;

using ItemPriceCharts.Infrastructure.Services;
using ItemPriceCharts.UI.WPF.Extensions;
using ItemPriceCharts.UI.WPF.Factories;
using ItemPriceCharts.UI.WPF.Services;
using ItemPriceCharts.UI.WPF.ViewModels.LoginAndRegistration;

namespace ItemPriceCharts.UI.WPF.Modules
{
    public class MappedTypeModules : Module
    {
        private readonly IDispatcherWrapper dispatcher;

        public MappedTypeModules(IDispatcherWrapper dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(this.dispatcher)
                .As<IDispatcherWrapper>()
                .SingleInstance();

            builder.RegisterType<HtmlWebWrapper>()
                .As<IHtmlWebWrapper>()
                .InstancePerDependency()
                .WithParameter(new TypedParameter(typeof(HtmlAgilityPack.HtmlWeb), new HtmlAgilityPack.HtmlWeb()));

            var mappedTypesInstancePerDependency = new Dictionary<Type, Type>
            {
                {typeof(ISystemDialogWrapper), typeof(SystemDialogWrapper)},
                {typeof(IFileSystemWrapper), typeof(FileSystemWrapper)},
                {typeof(IProcessWrapper), typeof(ProcessWrapper)},
                {typeof(IImageService), typeof(ImageService)},
            };

            var mappedTypesSingleInstance = new Dictionary<Type, Type>
            {
                {typeof(INavigationService<LoginViewModel>), typeof(NavigationService<LoginViewModel>)},
                {typeof(INavigationService<RegisterViewModel>), typeof(NavigationService<RegisterViewModel>)},
            };

            var mappedTypesPerScope = new Dictionary<Type, Type>
            {
                {typeof(ILogOutService), typeof(LogOutService)},
                {typeof(IItemService), typeof(ItemService)},
                {typeof(IOnlineShopService), typeof(OnlineShopService)},
                {typeof(IUserAccountService), typeof(UserAccountService)},
                {typeof(IItemDataRetrieveService), typeof(ItemDataRetrieveService)}
            };

            mappedTypesInstancePerDependency.ForEach((type, value) =>
                builder.RegisterType(value)
                    .As(type)
                    .InstancePerDependency());

            mappedTypesSingleInstance.ForEach((type, value) =>
                builder.RegisterType(value)
                    .As(type)
                    .SingleInstance());

            mappedTypesPerScope.ForEach((type, value) =>
                builder.RegisterType(value)
                    .As(type)
                    .InstancePerLifetimeScope());
        }
    }
}
