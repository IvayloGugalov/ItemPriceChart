using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ItemPriceCharts.UI.WPF.Factories
{
    public class DispatcherWrapper : IDispatcherWrapper
    {
        private readonly Dispatcher dispatcher;

        public int DispatcherId => this.dispatcher.Thread.ManagedThreadId;

        public DispatcherWrapper(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        }

        public Task InvokeAsync(Action callback)
        {
            if (Thread.CurrentThread.ManagedThreadId != this.DispatcherId)
            {
                throw new IncorrectThreadInvocationException(
                    $"Current thread is not the Dispatcher Thread. Current thread id: {Thread.CurrentThread.ManagedThreadId}");
            }

            return this.dispatcher.InvokeAsync(callback).Task;
        }

        public bool? Invoke(Func<bool?> callback)
        {
            if (Thread.CurrentThread.ManagedThreadId != this.DispatcherId)
            {
                throw new IncorrectThreadInvocationException(
                    $"Current thread is not the Dispatcher Thread. Current thread id: {Thread.CurrentThread.ManagedThreadId}");
            }

            return callback.Invoke();
        }

        private class IncorrectThreadInvocationException : InvalidOperationException
        {
            public IncorrectThreadInvocationException(string message) : base(message) { }
        }
    }
}