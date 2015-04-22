using System;
using System.Collections.Generic;
using System.Text;
using EnCor.Logging.Appenders;

namespace EnCor.Logging
{
    public class DefaultLoggingModule : LoggingModule
    {
        public DefaultLoggingModule()
            : base(new List<ILogAppender> { new ConsoleLogAppender(), new FileLogAppender("log/encor.log") })
        {
        }
    }
}
