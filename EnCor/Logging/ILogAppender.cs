namespace EnCor.Logging
{
    public interface ILogAppender
    {
        void Log(LogEntry logEntry);
    }
}
