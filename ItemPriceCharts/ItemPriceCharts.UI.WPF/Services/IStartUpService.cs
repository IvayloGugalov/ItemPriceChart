namespace ItemPriceCharts.UI.WPF.Services
{
    public interface IStartUpService
    {
        /// <summary>
        /// Determines which window to show on Start Up based on login information.
        /// </summary>
        void ShowStartUpWindow();
    }
}