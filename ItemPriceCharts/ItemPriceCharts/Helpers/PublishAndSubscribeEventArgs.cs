using System;
using System.Collections.Generic;

namespace ItemPriceCharts.UI.WPF.Helpers
{
    public delegate void PublishAndSubscribeEventHandler<T>(object sender, PublishAndSubscribeEventArgs<T> args);

    public class PublishAndSubscribeEventArgs<T> : EventArgs
    {
        /// <summary>
        /// The object to be recieved
        /// </summary>
        public T Item { get; set; }

        public PublishAndSubscribeEventArgs(T item)
        {
            this.Item = item;
        }
    }

    public static class PublishSubscribe<T>
    {
        private static readonly Dictionary<string, PublishAndSubscribeEventHandler<T>> events =
                            new Dictionary<string, PublishAndSubscribeEventHandler<T>>();

        public static void AddEvent(string name, PublishAndSubscribeEventHandler<T> handler)
        {
            if (!events.ContainsKey(name))
            {
                events.Add(name, handler);
            }
        }

        public static void Publish(string name, object sender, PublishAndSubscribeEventArgs<T> args)
        {
            if (events.ContainsKey(name) && events[name] != null)
            {
                events[name](sender, args);
            }
        }

        public static void RegisterEvent(string name, PublishAndSubscribeEventHandler<T> handler)
        {
            if (events.ContainsKey(name))
            {
                events[name] += handler;
            }
        }
    }
}
