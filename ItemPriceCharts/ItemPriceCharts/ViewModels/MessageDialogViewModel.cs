using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ItemPriceCharts.UI.WPF.Models;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class MessageDialogViewModel : BindableViewModel
    {
        public Message Message { get; }
        public ImageSource ImageSource { get; set; }
        public Action CloseWindow { get; set; }

        public MessageDialogViewModel(Message message)
        {
            this.Message = message;

            if (message.ImageSourceUri != null)
            {
                this.ImageSource = new BitmapImage(new Uri(message.ImageSourceUri, UriKind.Relative));
            }
        }

    }
}
