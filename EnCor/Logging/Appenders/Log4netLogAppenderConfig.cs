using System;
using EnCor.ObjectBuilder;
using System.Configuration;

namespace EnCor.Logging.Appenders
{
    public class Log4netLogAppenderConfig : LogAppenderConfig, IAssembler<ILogAppender, LogAppenderConfig>
    {
        private const string ConfigLoggerName = "logger";

        [ConfigurationProperty(ConfigLoggerName, DefaultValue = "all")]
        public string LoggerName
        {
            get
            {
                return (string) this[ConfigLoggerName];
            }
        }

        #region IAssembler<ILogAppender,LogAppenderConfig> Members

        public ILogAppender Assemble(IBuilderContext context, LogAppenderConfig objectConfiguration)
        {
            var config = objectConfiguration as Log4netLogAppenderConfig;
            if ( config == null)
            {
                throw new Exception(string.Format("Wrong config type for Log4netLogAppenderConfig, {0}", objectConfiguration));
            }
            return new Log4netLogAppender(config.LoggerName);
        }

        #endregion
    }
}
