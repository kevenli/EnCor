using System;
using System.Collections.Generic;
using System.Text;
using EnCor.Logging.Appenders;
using EnCor.ObjectBuilder;

namespace EnCor.Logging
{
    public class LogAppenderFactory : AssembleFactory<ILogAppender, LogAppenderConfig>
    {
    }
}
