using System;
using System.Threading.Tasks;

using ItemPriceCharts.UI.WPF.Factories;

namespace ItemPriceCharts.UI.WPF.Test.ViewModelTest
{
    public class TestableDispatcherWrapper : IDispatcherWrapper
    {
        public Task InvokeAsync(Action callback)
        {
            callback.Invoke();

            return Task.Delay(1);
        }

        public bool? Invoke(Func<bool?> callback)
        {
            return true;
        }
    }
}
