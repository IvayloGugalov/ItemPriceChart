using System;
using System.Threading.Tasks;

namespace ItemPriceCharts.UI.WPF.Factories
{
    /// <summary>
    /// Wrapper for the Dispatcher object
    /// </summary>
    public interface IDispatcherWrapper
    {
        Task InvokeAsync(Action callback);
    }
}
