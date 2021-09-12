using System;

using Autofac;
using Autofac.Core;
using NLog;

using ItemPriceCharts.Domain.Entities;
using ItemPriceCharts.Infrastructure.Services;
using ItemPriceCharts.UI.WPF.Events;
using ItemPriceCharts.UI.WPF.ViewModels;
using ItemPriceCharts.UI.WPF.ViewModels.LoginAndRegistration;
using ItemPriceCharts.UI.WPF.Views;
using ItemPriceCharts.XmReaderWriter.User;

namespace ItemPriceCharts.UI.WPF.Services
{
    public class StartUpService : IStartUpService, IDisposable
    {
        private static readonly Logger Logger = LogManager.GetLogger(nameof(StartUpService));
        
        private readonly App app;

        public StartUpService(App app)
        {
            this.app = app;
        }

        public void Dispose()
        {
            UiEvents.SuccessfulLogin.ClearHandlers();
        }

        public void ShowStartUpWindow()
        {
            try
            {
                var accountService = Bootstrapper.Bootstrapper.Resolve<IUserAccountService>();

                var userAccount = this.GetUserAccount(accountService);
                if (userAccount != null)
                {
                    this.ConfigureMainWindow(userAccount);
                    return;
                }

                // These can be empty, when the credentials are not saved or on first log in.
                string userName, email;
                (userName, email) = UserCredentialsSettings.UsernameAndEmail;
                Logger.Debug($"Found credentials\tUsername: {userName}\tEmail: {email}.");

                // pass the parameters to LoginViewModel
                Bootstrapper.Bootstrapper.Resolve<LoginViewModel>(
                    new NamedParameter(nameof(userName), userName),
                    new NamedParameter(nameof(email), email));

                var loginRegisterView = Bootstrapper.Bootstrapper.Resolve<LoginRegisterView>();
                var loginRegisterViewModel = Bootstrapper.Bootstrapper.Resolve<LoginRegisterViewModel>();

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
            var mainWindow = Bootstrapper.Bootstrapper.ViewFactory
                .Resolve<MainWindowViewModel>(
                    new Parameter[] { new TypedParameter(typeof(UserAccount), userAccount) });

            this.app.MainWindow = mainWindow ?? throw new NullReferenceException(nameof(mainWindow));
            this.app.MainWindow.Show();
            this.app.MainWindow.Activate();
        }

        private UserAccount GetUserAccount(IUserAccountService userAccountService)
        {
            return UserCredentialsSettings.ShouldEnableAutoLogin()
                ? userAccountService.GetUserAccount(UserCredentialsSettings.Username, UserCredentialsSettings.Email)
                    .GetAwaiter().GetResult()
                : null;
        }
    }
}