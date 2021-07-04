using System;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace ItemPriceCharts.UI.WPF.Factories
{
    public class DispatcherWrapper : IDispatcherWrapper
    {
        private readonly Dispatcher dispatcher;

        public DispatcherWrapper(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        }

        public Task InvokeAsync(Action callback)
        {
            return this.dispatcher.InvokeAsync(callback).Task;
        }
    }
}