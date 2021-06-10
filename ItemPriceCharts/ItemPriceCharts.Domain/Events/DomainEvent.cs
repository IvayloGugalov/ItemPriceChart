using System;
using System.Collections.Generic;

namespace ItemPriceCharts.Domain.Events
{
    public class DomainEvent<T> : IDomainEvent<T>
    {
        private readonly List<Delegate> handlers;

        public DomainEvent()
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
