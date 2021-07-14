using System;
using System.Threading.Tasks;

namespace ItemPriceCharts.UI.WPF.Factories
{
    /// <summary>
    /// Wrapper for the Dispatcher object
    /// </summary>
    public interface IDispatcherWrapper
    {
        /// <summary>
        /// Invokes the callback asynchronously on the UI thread.
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        Task InvokeAsync(Action callback);

        /// <summary>
        /// Invokes the callback on the UI thread and returns the boolean value from the function.
        /// </summary>
        /// <param name="callback"></param>
        /// <returns></returns>
        bool? Invoke(Func<bool?> callback);
    }
}
