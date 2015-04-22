using System;
using System.Configuration;
using EnCor.ObjectBuilder;

namespace EnCor.Logging.Appenders
{
    public class FileLogAppenderConfig : LogAppenderConfig, IAssembler<ILogAppender, LogAppenderConfig>
    {
        const string StrFilePath = "filePath";
        [ConfigurationProperty(StrFilePath, IsRequired = true)]
        public string FilePath
        {
            get
            {
                return (string)this[StrFilePath];
            }
        }
        #region IAssembler<ILogger,LoggerConfig> Members

        public ILogAppender Assemble(IBuilderContext context, LogAppenderConfig objectConfiguration)
        {
            if (objectConfiguration == null)
            {
                throw new ArgumentNullException("objectConfiguration");
            }
            FileLogAppenderConfig config = objectConfiguration as FileLogAppenderConfig;
            if (config == null)
            {
                throw new ArgumentException(string.Format("Config {0} is not FileLoggerConfig", objectConfiguration));
            }

            return new FileLogAppender(config.FilePath);
        }

        #endregion
    }
}
