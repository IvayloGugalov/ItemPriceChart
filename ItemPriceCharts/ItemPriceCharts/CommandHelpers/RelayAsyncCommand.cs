using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ItemPriceCharts.Helpers
{
    public interface IRelayAsyncCommand : ICommand
    {
        Task ExecuteAsync();
        bool CanExecute();
    }

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

    public class RelayAsyncCommand : IRelayAsyncCommand
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

        bool ICommand.CanExecute(object parameter)
        {
            return this.CanExecute();
        }

        void ICommand.Execute(object parameter)
        {
            this.ExecuteAsync().FireAndForgetSafeAsync(this.errorHandler);
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
    }

    #region AsyncCommand<T>
    //public interface IAsyncCommand<T> : ICommand
    //{
    //    Task ExecuteAsync(T parameter);
    //    bool CanExecute(T parameter);
    //}

    //public class AsyncCommand<T> : IAsyncCommand<T>
    //{
    //    public event EventHandler CanExecuteChanged;

    //    private bool isExecuting;
    //    private readonly Func<T, Task> execute;
    //    private readonly Func<T, bool> canExecute;
    //    private readonly IErrorHandler errorHandler;

    //    public AsyncCommand(Func<T, Task> execute, Func<T, bool> canExecute = null, IErrorHandler errorHandler = null)
    //    {
    //        this.execute = execute;
    //        this.canExecute = canExecute;
    //        this.errorHandler = errorHandler;
    //    }

    //    public bool CanExecute(T parameter)
    //    {
    //        return !this.isExecuting && (this.canExecute?.Invoke(parameter) ?? true);
    //    }

    //    public async Task ExecuteAsync(T parameter)
    //    {
    //        if (this.CanExecute(parameter))
    //        {
    //            try
    //            {
    //                this.isExecuting = true;
    //                await this.execute(parameter);
    //            }
    //            finally
    //            {
    //                this.isExecuting = false;
    //            }
    //        }

    //        this.RaiseCanExecuteChanged();
    //    }

    //    public void RaiseCanExecuteChanged()
    //    {
    //        this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    //    }

    //    bool ICommand.CanExecute(object parameter)
    //    {
    //        return this.CanExecute((T)parameter);
    //    }

    //    void ICommand.Execute(object parameter)
    //    {
    //        this.ExecuteAsync((T)parameter).FireAndForgetSafeAsync(this.errorHandler);
    //    }
    //}
    #endregion
}
