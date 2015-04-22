using EnCor.Configuration;
using EnCor.Logging.Appenders;

namespace EnCor.Logging
{
    public class AppenderConfigCollection : PolymorphicConfigurationElementCollection<LogAppenderConfig>
    {
        const string StrElementName = "appender";
        public AppenderConfigCollection()
        {
            AddElementName = StrElementName;
        }
    }
}
