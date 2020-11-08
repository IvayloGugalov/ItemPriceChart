using System;

namespace ItemPriceCharts.Services.Events
{
    public interface IChannel<T>
    {
        /// <summary>
        /// Subscribe to an event and recieve requested object
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IDisposable Subscribe(EventHandler<T> action);

        /// <summary>
        /// Publish an event and send requested object
        /// </summary>
        /// <param name="msg"></param>
        void Publish(T msg);

        /// <summary>
        /// Unsubscribe from an event
        /// </summary>
        /// <param name="unSubscriber"></param>
        void Unsubscribe(EventHandler<T> unSubscriber);
    }
}
