using System;

namespace ItemPriceCharts.UI.WPF.Helpers
{
    public class EventArgs<T> : EventArgs
    {
        public T Value { get; }

        public EventArgs(T value)
        {
            this.Value = value;
        }
    }
}
