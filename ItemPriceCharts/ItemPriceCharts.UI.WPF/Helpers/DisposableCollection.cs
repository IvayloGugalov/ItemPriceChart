using System;
using System.Collections.Generic;
using System.Threading;

namespace ItemPriceCharts.UI.WPF.Helpers
{
    public sealed class DisposableCollection : IDisposable
    {
        private List<IDisposable> disposables = new();

        public void Add(IDisposable disposable)
        {
            this.disposables.Add(disposable);
        }

        public void Dispose()
        {
            var disposables = Interlocked.Exchange(ref this.disposables, null);

            disposables?.ForEach(disposable => disposable.Dispose());
        }
    }
}
