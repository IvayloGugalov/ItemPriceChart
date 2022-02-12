using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

using ItemPriceCharts.UI.WPF.Helpers;

namespace ItemPriceCharts.UI.WPF.ViewModels.Base
{
    public abstract class BaseViewModel : IViewModel
    {
        private bool disposed;

        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetValue<TValue>(ref TValue target, TValue value, Action callback = null,
            [CallerMemberName] string propertyName = null)
        {
            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }
            if (Equals(target, value))
            {
                return;
            }

            target = value;

            this.OnPropertyChanged(propertyName);

            callback?.Invoke();
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> expression)
        {
            this.OnPropertyChanged(ExpressionHelper.PropertyName(expression));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                if (propertyName == null)
                {
                    throw new ArgumentNullException(nameof(propertyName));
                }

                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || this.disposed)
            {
                return;
            }

            this.disposed = true;
        }

        public virtual void Dispose()
        {
            this.Dispose(true);
        }
    }
}
