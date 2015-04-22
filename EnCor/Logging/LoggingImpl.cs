using System;
using System.Collections.Generic;
using System.Text;

namespace EnCor.Logging
{
    public class LoggingImpl : ILogging
    {
        private readonly IList<ILogAppender> _appenders = new List<ILogAppender>();
        public LoggingImpl(IEnumerable<ILogAppender> appenders)
        {
            foreach (var appender in appenders)
            {
                _appenders.Add(appender);
            }
        }

        #region ILogging Members

        public void Debug(string message)
        {
            Debug(message, null);
        }

        public void Debug(string message, Exception exception)
        {
            LogEntry logEntry = BuildLogEntry(LogLevel.Debug, message, exception);
            Log(logEntry);
        }

        public void Fatal(string message)
        {
            Fatal(message, null);
        }

        public void Fatal(string message, Exception exception)
        {
            LogEntry logEntry = BuildLogEntry(LogLevel.Fatal, message, exception);
            Log(logEntry);
        }

        public void Info(string message)
        {
            Info(message, null);
        }

        public void Info(string message, Exception exception)
        {
            LogEntry logEntry = BuildLogEntry(LogLevel.Information, message, exception);
            Log(logEntry);
        }

        public void Error(string message)
        {
            Error(message, null);
        }

        public void Error(string message, Exception exception)
        {
            LogEntry logEntry = BuildLogEntry(LogLevel.Error, message, exception);
            Log(logEntry);
        }

        public void Warn(string message)
        {
            Warn(message, null);
        }

        public void Warn(string message, Exception exception)
        {
            LogEntry logEntry = BuildLogEntry(LogLevel.Warning, message, exception);
            Log(logEntry);
        }

        protected LogEntry BuildLogEntry(LogLevel level, string message, Exception exception)
        {
            var entry = new LogEntry();
            entry.Level = level;
            entry.Message = message;
            entry.Exception = exception;
            return entry;
        }

        public void Log(LogEntry logEntry)
        {
            foreach (ILogAppender logger in _appenders)
            {
                logger.Log(logEntry);
            }
        }

        #endregion
    }
}
