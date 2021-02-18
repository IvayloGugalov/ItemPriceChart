using System;
using System.Threading.Tasks;

namespace ItemPriceCharts.UI.WPF.Helpers
{
    public static class TaskExtensions
    {
        public static async void FireAndForgetSafeAsync(this Task task, bool shouldAwait = true, Action<Exception> errorHandler = null)
        {
            try
            {
                await task.ConfigureAwait(shouldAwait);
            }
            catch (Exception ex) when (errorHandler != null)
            {
                errorHandler(ex);
            }
        }
    }
}
