using MaterialDesignThemes.Wpf;

namespace ItemPriceCharts.UI.WPF.Models
{
    public class Message
    {
        public string Title { get; }
        public string Description { get; }
        public string PositiveButtonText { get; }
        public string NegativeButtonText { get; }
        public string ImageSourceUri { get; }

        public bool IsSingleButtonShown { get; set; }
        public bool IsImageShown { get; }

        public Message(string title, string description, string positiveButtonText, string imageSourceUri = null)
        {
            this.Title = title;
            this.Description = description;
            this.PositiveButtonText = positiveButtonText;
            this.ImageSourceUri = imageSourceUri;
        }

        public Message(string title, string description, string positiveButtonText, string negativeButtonText, string imageSourceUri = null)
        {
            this.Title = title;
            this.Description = description;
            this.PositiveButtonText = positiveButtonText;
            this.NegativeButtonText = negativeButtonText;
            this.ImageSourceUri = imageSourceUri;

            this.IsImageShown = imageSourceUri != null;
        }
    }
}
