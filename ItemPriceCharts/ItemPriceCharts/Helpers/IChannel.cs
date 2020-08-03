using ItemPriceCharts.Services.Services;
using System;

namespace ItemPriceCharts.UI.WPF.Helpers
{
    public interface IChannel<T>
    {
        event EventHandler<MessageArgument<T>> Subscribe;
        void Publish(T data);
    }
}
