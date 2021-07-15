using System;

using ItemPriceCharts.Domain.Events;
using ItemPriceCharts.UI.WPF.Factories;

namespace ItemPriceCharts.UI.WPF.Events
{
    public class UiEvent<T> : IUiEvent<T>
    {
        private readonly Action<T> handler;
        private readonly IEvent<T> uiEvent;
        private readonly IDispatcherWrapper dispatcherWrapper;

        public UiEvent(IEvent<T> domainEvent, IDispatcherWrapper dispatcherWrapper)
        {
            this.uiEvent = new Event<T>();
            this.dispatcherWrapper = dispatcherWrapper;
            this.handler = message => this.uiEvent.Raise(message);
            domainEvent.Register(this.SendEventArgs);
        }

        public void Register(Action<T> action) => this.uiEvent.Register(action);

        private void SendEventArgs(T message)
        {
            this.dispatcherWrapper.InvokeAsync(() => this.handler(message));
        }
    }
}