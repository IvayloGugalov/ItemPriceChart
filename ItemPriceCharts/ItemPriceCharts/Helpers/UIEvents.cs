using ItemPriceCharts.Services.Services;
using ItemPriceCharts.UI.WPF.ViewModels;

namespace ItemPriceCharts.UI.WPF.Helpers
{
    public static class UIEvents
    {
        public static IChannel<IViewModel> CreateViewModel{ get; set; } = new Channel<IViewModel>();
    }
}
