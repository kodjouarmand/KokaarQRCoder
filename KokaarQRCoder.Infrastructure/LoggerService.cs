using System;
using KokaarQrCoder.Infrastructure.Contracts;
using KokaarQrCoder.Utility.Options;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using NLog;

namespace KokaarQrCoder.Infrastructure
{
    public class LoggerService : ILoggerService
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private readonly LoggingOptions _loggingSettings;

        public LoggerService(IOptions<LoggingOptions> loggingSettings, IWebHostEnvironment hostEnvironment)
        {
            _loggingSettings = loggingSettings.Value;
            var logPath = LoggingOptions.GetLogPath(hostEnvironment, _loggingSettings.LogFileName ?? LoggingOptions.DefaultLogFileName);

            var config = new NLog.Config.LoggingConfiguration();

            // Targets where to log to: File and Console
            var logfile = new NLog.Targets.FileTarget("logfile") { FileName = logPath };
            var logconsole = new NLog.Targets.ConsoleTarget("logconsole");

            // Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Info, LogLevel.Fatal, logconsole);
            config.AddRule(LogLevel.Debug, LogLevel.Fatal, logfile);

            // Apply config           
            LogManager.Configuration = config;
        }
        public void LogDebug(string message)
        {
            _logger.Debug($"--------------------------------------------DEBUG--------------------------------------------");
            _logger.Debug(message);
            _logger.Debug("\n");
        }
        public void LogError(Exception ex)
        {
            var exception = ex.InnerException ?? ex;
            _logger.Error($"--------------------------------------------ERROR--------------------------------------------");
            _logger.Error("**********************************ERROR DETAIL************************************");
            _logger.Error($"MESSAGE : {exception.Message}");
            _logger.Error($"SOURCE : {exception.Source}");
            _logger.Error($"STACK TRACE : {exception.StackTrace}");
            _logger.Error("**********************************END ERROR DETAIL********************************");
            _logger.Error("\n");
        }
        public void LogError(string message)
        {
            _logger.Error($"--------------------------------------------ERROR--------------------------------------------");
            _logger.Error(message);
            _logger.Error("\n");
        }
        public void LogInformation(string message)
        {
            _logger.Info($"--------------------------------------------INFO--------------------------------------------");
            _logger.Info(message);
            _logger.Info("\n");
        }
        public void LogWarning(string message)
        {
            _logger.Warn($"--------------------------------------------WARN--------------------------------------------");
            _logger.Warn(message);
            _logger.Warn("\n");
        }

    }
}
