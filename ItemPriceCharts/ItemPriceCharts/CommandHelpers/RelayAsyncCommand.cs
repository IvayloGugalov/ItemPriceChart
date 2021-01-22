using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ItemPriceCharts.UI.WPF.CommandHelpers
{
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }

    public static class TaskUtilities
    {
        public static async void FireAndForgetSafeAsync(this Task task, IErrorHandler errorHandler = null)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                errorHandler?.HandleError(ex);
            }
        }
    }

    public class RelayAsyncCommand : ICommand
    {
        private bool isExecuting;
        private readonly Func<Task> execute;
        private readonly Func<bool> canExecute;
        private readonly IErrorHandler errorHandler;

        public event EventHandler CanExecuteChanged;

        public RelayAsyncCommand(Func<Task> execute, Func<bool> canExecute = null, IErrorHandler errorHandler = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
            this.errorHandler = errorHandler;
        }

        public bool CanExecute()
        {
            return !this.isExecuting && (this.canExecute?.Invoke() ?? true);
        }

        public async Task ExecuteAsync()
        {
            if (this.CanExecute())
            {
                try
                {
                    this.isExecuting = true;
                    await this.execute();
                }
                finally
                {
                    this.isExecuting = false;
                }
            }

            this.RaiseCantExecuteChanged();
        }

        public void RaiseCantExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        #region Explicit implementations
        bool ICommand.CanExecute(object parameter)
        {
            return this.CanExecute();
        }

        void ICommand.Execute(object parameter)
        {
            this.ExecuteAsync().FireAndForgetSafeAsync(this.errorHandler);
        }
        #endregion
    }
}
