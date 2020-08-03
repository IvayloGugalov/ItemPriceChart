using System;

namespace ItemPriceCharts.Services.Services
{
    public class Channel<T> : IChannel<T>
    {
        private readonly object myLock = new object();
        private event EventHandler<MessageArgument<T>> Subscribers;

        public void Publish(T msg)
        {
            lock(myLock)
            {
                var message = (MessageArgument<T>)Activator.CreateInstance(typeof(MessageArgument<T>), new object[] { msg });
                this.Subscribers?.Invoke(this, message);
            }
        }

        public IDisposable Subscribe(EventHandler<MessageArgument<T>> action)
        {
            return this.SubscribeOnThread(action);
        }

        private IDisposable SubscribeOnThread(EventHandler<MessageArgument<T>> subscriber)
        {
            lock (myLock)
            {
                this.Subscribers += subscriber;
            }

            return new Unsubscriber<T>(subscriber, this);
        }

        public void Unsubscribe(EventHandler<MessageArgument<T>> unSubscriber)
        {
            lock (myLock)
            {
                this.Subscribers -= unSubscriber;
            }
        }
    }

    internal class Unsubscriber<T> : IDisposable
    {
        readonly EventHandler<MessageArgument<T>> receiver;
        readonly Channel<T> channel;

        public Unsubscriber(EventHandler<MessageArgument<T>> receiver, Channel<T> channel)
        {
            this.receiver = receiver;
            this.channel = channel;
        }

        public void Dispose()
        {
            this.channel.Unsubscribe(this.receiver);
        }
    }

    public class MessageArgument<T> : EventArgs
    {
        public T Message { get; }

        public MessageArgument(T message)
        {
            this.Message = message;
        }
    }
}
