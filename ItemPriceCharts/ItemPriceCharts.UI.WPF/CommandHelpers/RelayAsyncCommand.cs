using System;
using System.Threading.Tasks;
using System.Windows.Input;

using ItemPriceCharts.UI.WPF.Helpers;

namespace ItemPriceCharts.UI.WPF.CommandHelpers
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync();
    }

    public class RelayAsyncCommand : IAsyncCommand
    {
        private readonly Func<Task> execute;
        private readonly Func<bool> canExecute;
        private readonly Action<Exception> errorHandler;
        private readonly bool continueOnCapturedContext;

        private event EventHandler InternalCanExecuteChanged;

        private bool isExecuting;

        public RelayAsyncCommand(Func<Task> execute, Func<bool> canExecute = null, Action<Exception> errorHandler = null, bool continueOnCapturedContext = true)
        {
            this.execute = execute;
            this.canExecute = canExecute;
            this.errorHandler = errorHandler;
            this.continueOnCapturedContext = continueOnCapturedContext;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                this.InternalCanExecuteChanged += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
                this.InternalCanExecuteChanged -= value;
            }
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

        protected void RaiseCantExecuteChanged()
        {
            this.InternalCanExecuteChanged.Raise(this);
        }

        #region Explicit implementations
        bool ICommand.CanExecute(object parameter)
        {
            return this.CanExecute();
        }

        void ICommand.Execute(object parameter)
        {
            this.ExecuteAsync().FireAndForgetSafeAsync(this.continueOnCapturedContext, this.errorHandler);
        }
        #endregion
    }
}
