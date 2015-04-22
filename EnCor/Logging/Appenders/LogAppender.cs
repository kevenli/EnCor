using EnCor.ObjectBuilder;

namespace EnCor.Logging.Appenders
{
    [AssembleConfig(typeof(LogAppenderConfig))]
    public abstract class LogAppender : ILogAppender
    {
        public abstract void Log(LogEntry logEntry);
    }
}
