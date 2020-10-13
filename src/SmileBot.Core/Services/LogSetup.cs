using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace SmileBot.Core.Services
{
    class LogSetup
    {
        public static void SetupLogger(int shardId)
        {
            var logConfig = new LoggingConfiguration();
            var consoleTarget = new ColoredConsoleTarget()
            {
                Layout = shardId + @" ${date:format=HH\:mm\:ss} ${logger:shortName=True} | ${message}"
            };

            logConfig.AddTarget("Console", consoleTarget);

            logConfig.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, consoleTarget));

            LogManager.Configuration = logConfig;
        }
    }
}