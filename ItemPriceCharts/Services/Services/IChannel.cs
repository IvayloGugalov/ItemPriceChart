using System;

namespace ItemPriceCharts.Services.Services
{
    public interface IChannel<T>
    {
        /// <summary>
        /// Subscribe to an event end recieve a generic message
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        IDisposable Subscribe(EventHandler<MessageArgument<T>> action);

        /// <summary>
        /// Publish an event and send a generic message
        /// </summary>
        /// <param name="msg"></param>
        void Publish(T msg);

        /// <summary>
        /// Unsubscribe from an event
        /// </summary>
        /// <param name="unSubscriber"></param>
        void Unsubscribe(EventHandler<MessageArgument<T>> unSubscriber);
    }
}
