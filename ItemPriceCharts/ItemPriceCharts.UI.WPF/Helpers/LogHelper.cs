﻿using System;
using System.Collections.Generic;
using System.Linq;

using NLog;
using NLog.Targets;

using ItemPriceCharts.UI.WPF.Extensions;

namespace ItemPriceCharts.UI.WPF.Helpers
{
    public static class LogHelper
    {
        private const string EXPLORER_PROCESS = "explorer.exe";
        private const string LOG_FILE_NAME = "logFile";

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
            var disableLevels = AllLevels.Where(x => x < level)
                .ToArray();

            var enableLevels = AllLevels.Where(x => x >= level)
                .ToArray();

            foreach (var rule in LogManager.Configuration.LoggingRules)
            {
                disableLevels.ForEach(rule.DisableLoggingForLevel);
                enableLevels.ForEach(rule.EnableLoggingForLevel);
            }

            LogManager.ReconfigExistingLoggers();
        }

        public static void OpenLogFolder()
        {
            System.Diagnostics.Process.Start(LogHelper.EXPLORER_PROCESS, LogFolder);
        }

        private static string GetLogFolder()
        {
            var fileTarget = (FileTarget)LogManager.Configuration.FindTargetByName(LogHelper.LOG_FILE_NAME);
            var logEventInfo = new LogEventInfo { TimeStamp = DateTime.UtcNow };
            var filePath = fileTarget.FileName.Render(logEventInfo);

            return System.IO.Path.GetDirectoryName(filePath);
        }
    }
}
