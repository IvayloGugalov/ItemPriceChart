namespace ItemPriceCharts.UI.WPF.Services
{
    public interface ILogOutService
    {
        /// <summary>
        /// Invokes the deletion of current login information and shuts down application.
        /// </summary>
        void LogOut();
    }
}