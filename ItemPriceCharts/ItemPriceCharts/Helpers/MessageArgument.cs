using System;

namespace ItemPriceCharts.UI.WPF.Helpers
{
    public class MessageArgument<T> : EventArgs
    {
        public T Message { get; set; }
        public MessageArgument(T message)
        {
            this.Message = message;
        }
    }

    public interface IPublisher<T>
    {
        event EventHandler<MessageArgument<T>> OnDataPublished;
        void OnDataPublisher(MessageArgument<T> args);
        void Publish(T data);
    }

    public class Publisher<T> : IPublisher<T>
    {
        //Defined datapublisher event  
        public event EventHandler<MessageArgument<T>> OnDataPublished;

        public void OnDataPublisher(MessageArgument<T> args)
        {
            this.OnDataPublished?.Invoke(this, args);
        }

        public void Publish(T data)
        {
            MessageArgument<T> message = (MessageArgument<T>)Activator.CreateInstance(typeof(MessageArgument<T>), new object[] { data });

            this.OnDataPublisher(message);
        }
    }

    public class Subscriber<T>
    {
        public IPublisher<T> Publisher { get; private set; }
        public Subscriber(IPublisher<T> publisher)
        {
            this.Publisher = publisher;
        }
    }

    public static class NewEvents
    {
        public static Publisher<string> NewShopAddedPub { get; set; } = new Publisher<string>();
        public static Subscriber<string> NewShopAddedSub { get; set; } = new Subscriber<string>(NewShopAddedPub);

    }
}
