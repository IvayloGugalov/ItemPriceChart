using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ItemPriceCharts.UI.WPF.ViewModels
{
    public class BindableViewModel : IViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void SetValue<TValue>(ref TValue target, TValue value, Action callback = null,
            [CallerMemberName] string propertyName = null)
        {
            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }
            if (object.Equals(target, value))
            {
                return;
            }

            target = value;

            this.OnPropertyChanged(propertyName);

            callback?.Invoke();
        }

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            PropertyChanged?.Invoke
                (this,
                new PropertyChangedEventArgs(propertyName));
        }
    }
}
