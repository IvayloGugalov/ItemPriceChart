using System;
using System.Threading.Tasks;

namespace ItemPriceCharts.UI.WPF.Helpers
{
    public static class TaskExtensions
    {
        public static async void FireAndForgetSafeAsync(this Task task, bool continueOnCapturedContext = true, Action<Exception> errorHandler = null)
        {
            try
            {
                await task.ConfigureAwait(continueOnCapturedContext);
            }
            catch (Exception ex)
            {
                if (errorHandler is null)
                {
                    throw;
                }

                errorHandler?.Invoke(ex);
            }
        }
    }
}
