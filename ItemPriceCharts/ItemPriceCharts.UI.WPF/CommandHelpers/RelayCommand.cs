using System;
using System.Windows.Input;

using ItemPriceCharts.UI.WPF.Helpers;

namespace ItemPriceCharts.UI.WPF.CommandHelpers
{
    public class RelayCommand : RelayCommand<object>
    {
        public RelayCommand(Action<object> action) : base(action)
        {
        }

        public RelayCommand(Action<object> action, Func<bool> predicate) : base(action, predicate)
        {
        }
    }

    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> execute = null;
        private readonly Func<bool> canExecute = null;

        protected RelayCommand(Action<T> execute, Func<object> predicate) : this(execute, null)
        {
        }

        public RelayCommand(Action<T> execute, Func<bool> canExecute = null)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
        }

        private event EventHandler CanExecuteChangedInternal;

        public event EventHandler CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
                CanExecuteChangedInternal += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
                CanExecuteChangedInternal -= value;
            }
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute?.Invoke() ?? true;
        }

        public void Execute(object parameter)
        {
            this.execute((T)parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            this.CanExecuteChangedInternal.Raise(this);
        }
    }
}
