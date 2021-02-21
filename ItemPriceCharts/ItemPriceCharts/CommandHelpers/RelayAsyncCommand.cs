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

        private bool isExecuting;

        public event EventHandler CanExecuteChanged;

        public RelayAsyncCommand(Func<Task> execute, Func<bool> canExecute = null, Action<Exception> errorHandler = null)
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
            this.CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        #region Explicit implementations
        bool ICommand.CanExecute(object parameter)
        {
            return this.CanExecute();
        }

        void ICommand.Execute(object parameter)
        {
            this.ExecuteAsync().FireAndForgetSafeAsync(true, this.errorHandler);
        }
        #endregion
    }
}
