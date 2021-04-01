using System;

namespace ItemPriceCharts.Services.Events
{
    public class Channel<T> : IChannel<T>
    {
        private event EventHandler<T> Subscribers;

        public void Publish(T msg)
        {
            this.Subscribers?.Invoke(this, msg);
        }

        public IDisposable Subscribe(EventHandler<T> action)
        {
            this.Subscribers += action;

            return new Unsubscriber<T>(action, this);
        }

        public void Unsubscribe(EventHandler<T> unSubscriber)
        {
            this.Subscribers -= unSubscriber;
        }
    }

    internal class Unsubscriber<T> : IDisposable
    {
        readonly EventHandler<T> receiver;
        readonly Channel<T> channel;

        public Unsubscriber(EventHandler<T> receiver, Channel<T> channel)
        {
            this.receiver = receiver;
            this.channel = channel;
        }

        public void Dispose()
        {
            this.channel.Unsubscribe(this.receiver);
        }
    }

    //public class MessageArgument<T> : EventArgs
    //{
    //    public T Message { get; }

    //    public MessageArgument(T message)
    //    {
    //        this.Message = message;
    //    }
    //}
}
