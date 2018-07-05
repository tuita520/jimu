﻿using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using NLog.Targets;

namespace Jimu
{
    public class NLogger : ILogger
    {
        private readonly Logger _logger;

        public NLogger(LogOptions options = null)
        {
            options = options ?? new LogOptions { EnableConsoleLog = true, ConsoleLogLevel = LogLevel.Error | LogLevel.Info | LogLevel.Warn };
            var config = new NLog.Config.LoggingConfiguration();
            if (options.EnableFileLog)
            {
                var fileConf = new NLog.Targets.FileTarget("logFile")
                {
                    FileName = ".\\log\\${level:lowercase=true}\\${shortdate}.log",
                    ArchiveAboveSize = 16000000,
                    Layout = @"${date:format=yyyy-MM-dd HH\:mm\:ss.fff} ${level:uppercase=true}  ${message}"
                };
                if ((options.FileLogLevel & LogLevel.Error) == LogLevel.Error)
                {
                    config.AddRuleForOneLevel(NLog.LogLevel.Error, fileConf);
                }
                if ((options.FileLogLevel & LogLevel.Warn) == LogLevel.Warn)
                {
                    config.AddRuleForOneLevel(NLog.LogLevel.Warn, fileConf);
                }
                if ((options.FileLogLevel & LogLevel.Info) == LogLevel.Info)
                {
                    config.AddRuleForOneLevel(NLog.LogLevel.Info, fileConf);
                }
            }

            if (options.EnableConsoleLog)
            {
                var consoleLog = new NLog.Targets.ConsoleTarget("logconsole")
                {
                    Layout = @"${date:format=yyyy-MM-dd HH\:mm\:ss.fff} ${level:uppercase=true}  ${message}"
                };
                if ((options.ConsoleLogLevel & LogLevel.Error) == LogLevel.Error)
                {
                    config.AddRuleForOneLevel(NLog.LogLevel.Error, consoleLog);
                }
                if ((options.FileLogLevel & LogLevel.Warn) == LogLevel.Warn)
                {
                    config.AddRuleForOneLevel(NLog.LogLevel.Warn, consoleLog);
                }
                if ((options.FileLogLevel & LogLevel.Info) == LogLevel.Info)
                {
                    config.AddRuleForOneLevel(NLog.LogLevel.Info, consoleLog);
                }
            }
            NLog.LogManager.Configuration = config;
            _logger = NLog.LogManager.GetLogger("logger");
        }
        public void Info(string info)
        {
            _logger.Info(info);
        }

        public void Warn(string info)
        {
            _logger.Warn(info);
        }

        public void Error(string info, Exception ex)
        {
            _logger.Error(ex, info);
        }
    }
}