using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;

namespace ItemPriceCharts.UI.WPF.Helpers
{
    public static class Schedular
    {
        public static IDisposable Schedule(this IScheduler scheduler, TimeSpan timeSpan, Action action)
        {
            return scheduler.Schedule<object>(
                null,
                timeSpan,
                (a1, a2) =>
                    {
                        action();
                        return Disposable.Empty;
                    });
        }

    }
}
