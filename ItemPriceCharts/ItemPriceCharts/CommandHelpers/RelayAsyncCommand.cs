using System;
using System.Threading.Tasks;
using System.Windows.Input;

using static ItemPriceCharts.UI.WPF.Helpers.TaskExtensions;

namespace ItemPriceCharts.UI.WPF.CommandHelpers
{
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
            this.ExecuteAsync().FireAndForgetSafeAsync(true, this.errorHandler);
        }
        #endregion
    }
}
