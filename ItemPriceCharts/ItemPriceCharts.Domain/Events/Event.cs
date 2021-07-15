using System;
using System.Collections.Generic;

namespace ItemPriceCharts.Domain.Events
{
    public class Event<T> : IEvent<T>
    {
        private readonly List<Delegate> handlers;

        public Event()
        {
            this.handlers = new List<Delegate>();
        }

        public void Register(Action<T> eventHandler) => this.handlers.Add(eventHandler);

        public void Raise(T domainEvent)
        {
            foreach (var handler in this.handlers)
            {
                var action = (Action<T>)handler;

                action.Invoke(domainEvent);
            }
        }

        public void ClearHandlers() => this.handlers.Clear();
    }
}
