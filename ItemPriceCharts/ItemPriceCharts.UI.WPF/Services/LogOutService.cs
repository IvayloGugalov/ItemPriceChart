using ItemPriceCharts.UI.WPF.Events;
using ItemPriceCharts.XmReaderWriter.User;

namespace ItemPriceCharts.UI.WPF.Services
{
    public class LogOutService : ILogOutService
    {

        public void LogOut()
        {
            UserCredentialsSettings.RemoveUserDetailsFromXmlFile();

            UiEvents.CloseApplication();
        }
    }
}
