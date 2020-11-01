using System;
using System.Windows.Media;

using MaterialDesignThemes.Wpf;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class MessageDialogViewModel : BindableViewModel
    {
        private const string ACCEPT_TEXT = "Accept";
        private const string CLOSE_TEXT = "Close";
        private const string CANCEL_TEXT = "Cancel";

        public Action CloseWindow { get; set; }

        public string Title { get; }
        public string Description { get; }
        public PackIconKind IconType { get; }
        public Brush IconColor { get; }

        public string NegativeButtonText { get; }
        public string PositiveButtonText { get; }

        public bool IsSingleButtonShown { get; }
        public bool IsImageShown { get; }

        public MessageDialogViewModel(string title, string description, ButtonType buttonType)
        {
            this.Title = title;
            this.Description = description;
            this.IconType = PackIconKind.Exclamation;

            switch (buttonType)
            {
                case ButtonType.Close:
                    this.NegativeButtonText = CLOSE_TEXT;
                    this.IsSingleButtonShown = true;
                    this.IconColor = Brushes.IndianRed;
                    break;
                case ButtonType.OkCancel:
                    this.PositiveButtonText = ACCEPT_TEXT;
                    this.NegativeButtonText = CANCEL_TEXT;
                    this.IconColor = Brushes.Aqua;
                    break;
                default:
                    break;
            }
        }
    }

    public enum ButtonType
    {
        Close = 1,
        OkCancel = 2
    }
}
