using System;
using EnCor.ObjectBuilder;

namespace EnCor.Logging.Appenders
{
    public class LogAppenderAssembler : IAssembler<ILogAppender, LogAppenderConfig>
    {
        public virtual ILogAppender Assemble(IBuilderContext context, LogAppenderConfig objectConfiguration)
        {
            ILogAppender appender = (ILogAppender)Activator.CreateInstance(objectConfiguration.Type);
            return appender;
        }
    }
}
