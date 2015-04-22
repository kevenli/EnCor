using System;
using System.Collections.Generic;
using System.Text;
using EnCor.Logging.Appenders;

namespace EnCor.Logging
{
    public class LoggingFactory
    {
        private static ILogging LoggingCache;

        public static ILogging GetLogging()
        {
            if (LoggingCache == null)
            {
                IList<ILogAppender> appenders = new List<ILogAppender>();
                appenders.Add(new ConsoleLogAppender());
                appenders.Add(new FileLogAppender("log/encor.log"));
                var logging = new LoggingImpl(appenders);
                LoggingCache = logging;
            }

            return LoggingCache;
        }
    }
}
