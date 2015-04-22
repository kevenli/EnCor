using System;
using System.Collections.Generic;

namespace EnCor.Logging
{
    public class LoggingModule : Module
    {
        private ILogging _LoggingImpl;
        public LoggingModule(IEnumerable<ILogAppender> appenders )
        {
            _LoggingImpl = new LoggingImpl(appenders);
        }

        public override void Init()
        {
            RegisterBizService(_LoggingImpl, "logging", typeof(ILogging));
        }

        
    }
}
