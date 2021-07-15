using System;

namespace ItemPriceCharts.UI.WPF.Events
{
    public interface IUiEvent<out T>
    {
        void Register(Action<T> action);
    }
}