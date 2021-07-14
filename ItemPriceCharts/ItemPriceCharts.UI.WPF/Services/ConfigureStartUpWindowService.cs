using System;

using Autofac;
using Autofac.Core;
using NLog;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.Infrastructure.Services;
using ItemPriceCharts.UI.WPF.Events;
using ItemPriceCharts.UI.WPF.Factories;
using ItemPriceCharts.UI.WPF.ViewModels;
using ItemPriceCharts.UI.WPF.ViewModels.LoginAndRegistration;
using ItemPriceCharts.UI.WPF.Views;
using ItemPriceCharts.XmReaderWriter.User;

namespace ItemPriceCharts.UI.WPF.Services
{
    public class ConfigureStartUpWindowService
    {
        private readonly IContainer container;
        private readonly App app;
        private readonly IViewFactory viewFactory;
        private static readonly Logger Logger = LogManager.GetLogger(nameof(ConfigureStartUpWindowService));

        public ConfigureStartUpWindowService(App app, IContainer container, IViewFactory viewFactory)
        {
            this.app = app;
            this.container = container;
            this.viewFactory = viewFactory;
        }

        public void ShowStartUpWindow()
        {
            try
            {
                var accountService = this.container.Resolve<IUserAccountService>();

                var userAccount = this.TryGetUserAccount(accountService);
                if (userAccount != null)
                {
                    this.ConfigureMainWindow(userAccount);
                    UiEvents.SuccessfulLogin = null;
                    return;
                }

                // These can be empty, when the credentials are not saved or on first log in.
                string userName, email;
                (userName, email) = UserCredentialsSettings.UsernameAndEmail;
                Logger.Debug($"Found credentials\tUsername: {userName}\tEmail: {email}.");

                // pass the parameters to LoginViewModel
                this.container.Resolve<LoginViewModel>(
                    new NamedParameter(nameof(userName), userName),
                    new NamedParameter(nameof(email), email));

                var loginRegisterView = this.container.Resolve<LoginRegisterView>();
                var loginRegisterViewModel = this.container.Resolve<LoginRegisterViewModel>();

                UiEvents.SuccessfulLogin.Register(this.ConfigureMainWindow);

                loginRegisterView.DataContext = loginRegisterViewModel;
                loginRegisterView.ShowDialog();
            }
            catch (Exception e)
            {
                Logger.Error(e.Message, "Could not show window");
                throw new Exception("We are having difficulties with the app, please send us the logs!");
            }
        }

        private void ConfigureMainWindow(UserAccount userAccount)
        {
            var mainWindow = this.viewFactory.Resolve<MainWindowViewModel>(new Parameter[] { new TypedParameter(typeof(UserAccount), userAccount) });
            this.app.MainWindow = mainWindow;
            this.app.MainWindow?.Show();
        }

        private UserAccount TryGetUserAccount(IUserAccountService userAccountService)
        {
            return UserCredentialsSettings.ShouldEnableAutoLogin()
                ? userAccountService.GetUserAccount(UserCredentialsSettings.Username, UserCredentialsSettings.Email)
                    .GetAwaiter().GetResult()
                : null;
        }
    }
}