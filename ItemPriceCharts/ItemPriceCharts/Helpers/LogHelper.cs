using System;
using System.Collections.Generic;
using System.Linq;

using NLog;
using NLog.Targets;

namespace ItemPriceCharts.UI.WPF.Helpers
{
    public static class LogHelper
    {
        private static string logFolder;

        private static readonly IEnumerable<LogLevel> AllLevels = new[]
        {
            LogLevel.Trace,
            LogLevel.Debug,
            LogLevel.Info,
            LogLevel.Warn,
            LogLevel.Error,
            LogLevel.Fatal,
        };

        public static string LogFolder
        {
            get
            {
                if (!string.IsNullOrEmpty(logFolder))
                {
                    return logFolder;
                }

                logFolder = GetLogFolder();
                return logFolder;
            }
        }

        public static void ReconfigureLoggerToLevel(LogLevel level)
        {
            GlobalDiagnosticsContext.Set("Application", "My cool app");
            GlobalDiagnosticsContext.Set("Version", "1.0.42");

            var disableLevels = AllLevels.Where(x => x < level)
                .ToArray();

            var enableLevels = AllLevels.Where(x => x >= level)
                .ToArray();

            foreach (var rule in LogManager.Configuration.LoggingRules)
            {
                var localRule = rule;

                disableLevels.ForEach(localRule.DisableLoggingForLevel);
                enableLevels.ForEach(localRule.EnableLoggingForLevel);
            }

            LogManager.ReconfigExistingLoggers();
        }

        private static IEnumerable<T> ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (var i in enumerable)
            {
                action(i);
            }

            return enumerable;
        }

        public static void OpenLogFolder()
        {
            System.Diagnostics.Process.Start("explorer.exe", LogFolder);
        }

        private static string GetLogFolder()
        {
            var fileTarget = (FileTarget)LogManager.Configuration.FindTargetByName("logFile");
            var logEventInfo = new LogEventInfo { TimeStamp = DateTime.Now };
            var filePath = fileTarget.FileName.Render(logEventInfo);

            return System.IO.Path.GetDirectoryName(filePath);
        }
    }
}
