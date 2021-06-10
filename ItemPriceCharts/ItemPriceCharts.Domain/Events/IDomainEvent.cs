using System;

namespace ItemPriceCharts.Domain.Events
{
    public interface IDomainEvent<T>
    {
        void Register(Action<T> eventHandler);
        void Raise(T domainEvent);
        void ClearHandlers();
    }
}