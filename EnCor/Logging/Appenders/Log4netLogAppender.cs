using EnCor.ObjectBuilder;
using log4net;
using log4net.Core;

namespace EnCor.Logging.Appenders
{
    [AssembleConfig(typeof(Log4netLogAppenderConfig))]
    public class Log4netLogAppender : LogAppender
    {
        private ILog logger;
        public Log4netLogAppender(string loggerName)
        {
            log4net.Config.XmlConfigurator.Configure();
            logger = LogManager.GetLogger(loggerName);
        }

        public override void Log(LogEntry logEntry)
        {
            LoggingEventData loggingEventData = new LoggingEventData();
            loggingEventData.Domain = logEntry.AppDomainName;
            if (logEntry.Exception != null)
            {
                loggingEventData.ExceptionString = logEntry.Exception.ToString();
            }
            if (logEntry.Level == LogLevel.Debug)
                loggingEventData.Level = Level.Debug;
            if (logEntry.Level == LogLevel.Error)
                loggingEventData.Level = Level.Error;
            if (logEntry.Level == LogLevel.Fatal)
                loggingEventData.Level = Level.Fatal;
            if (logEntry.Level == LogLevel.Information)
                loggingEventData.Level = Level.Info;
            if (logEntry.Level == LogLevel.Warning)
                loggingEventData.Level = Level.Warn;

            loggingEventData.Message = logEntry.Message;
            loggingEventData.TimeStamp = logEntry.TimeStamp;
            loggingEventData.ThreadName = logEntry.ThreadName;

            LoggingEvent loggingEvent = new LoggingEvent(loggingEventData);
            logger.Logger.Log(loggingEvent);
        }
    }
}
