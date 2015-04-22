using System;

namespace EnCor.Logging.Appenders
{
    public class ConsoleLogAppender : LogAppender
    {
        public override void Log(LogEntry logEntry)
        {
            Console.WriteLine(string.Format("date:{0} \r\nthread:{1} \r\nloglevel:{2} \r\nlogger:{3} \r\nmessage:{4}",
            logEntry.TimeStamp,
            logEntry.ThreadName,
            logEntry.Level,
            logEntry.LoggerName,
            logEntry.Message));
        }
    }
}
