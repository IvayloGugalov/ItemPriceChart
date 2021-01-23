using System;
using System.Threading.Tasks;

namespace ItemPriceCharts.UI.WPF.Helpers
{
    public static class TaskExtensions
    {
        public interface IErrorHandler
        {
            void HandleError(Exception ex);
        }

        public static async void FireAndForgetSafeAsync(this Task task, bool shouldAwait = true, IErrorHandler errorHandler = null)
        {
            try
            {
                await task.ConfigureAwait(shouldAwait);
            }
            catch (Exception ex) when (errorHandler != null)
            {
                errorHandler?.HandleError(ex);
            }
        }
    }
}
