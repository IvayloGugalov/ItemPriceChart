using ItemPriceCharts.UI.WPF.Events;
using ItemPriceCharts.UI.WPF.ViewModels;

namespace ItemPriceCharts.UI.WPF.Helpers
{
    public static class MessageDialogCreator
    {
        public static void ShowErrorDialog(
            string message,
            string title = "Error",
            ButtonType buttonType = ButtonType.Close)
        {
            UiEvents.ShowMessageDialog(
                new MessageDialogViewModel(
                    title: title,
                    description: message,
                    buttonType: buttonType));
        }
    }
}
