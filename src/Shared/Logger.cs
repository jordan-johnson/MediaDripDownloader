using System;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.File;
using Serilog.Sinks.SystemConsole;

using LoggingLevel = Serilog.Events.LogEventLevel;

namespace MediaDrip.Downloader.Shared
{
    public enum LoggingPlatform
    {
        Console,
        File
    }

    public class LoggingOptions
    {
        public LoggingPlatform Platform;
        public LoggingLevel MinimumLevel;
        public String FileLocation;
    }

    public class Logger
    {
        private LoggerConfiguration _config;
        private LoggingOptions _options;

        public Logger(LoggingOptions options = null)
        {
            _config = new LoggerConfiguration();
            _options = options ?? GenerateDefaultOptions();

            InitializeLevelSwitch();
            InitalizeLogPlatform();
            CreateLogger();
        }

        private LoggingOptions GenerateDefaultOptions()
        {
            return new LoggingOptions
            {
                Platform = LoggingPlatform.Console,
                MinimumLevel = LoggingLevel.Information
            };
        }

        private void InitializeLevelSwitch()
        {
            var levelSwitch = new LoggingLevelSwitch() { MinimumLevel = _options.MinimumLevel };

            _config?.MinimumLevel.ControlledBy(levelSwitch);
        }

        private void InitalizeLogPlatform()
        {
            switch(_options.Platform)
            {
                case LoggingPlatform.Console:
                    _config?.WriteTo.Console();
                break;
                case LoggingPlatform.File:
                    _config?.WriteTo.File(_options.FileLocation);
                break;
                default:
                    _config.WriteTo.Console();
                break;
            }
        }

        private void CreateLogger() => _config.CreateLogger();
    }
}