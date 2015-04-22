using System.Configuration;
using EnCor.Configuration;
using EnCor.ModuleLoader;
using System.Collections.Generic;

namespace EnCor.Logging
{
    public class LoggingModuleConfig : ModuleConfig , IModuleAssembler
    {
        private const string ConfigAppenders = "appenders";

        [ConfigurationProperty(ConfigAppenders)]
        public AppenderConfigCollection Appenders
        {
            get
            {
                return (AppenderConfigCollection) this[ConfigAppenders];
            }
        }

        #region IAssembler<IEnCorModule,ModuleConfig> Members

        public IEnCorModule Assemble(ObjectBuilder.IBuilderContext context, IModuleConfig objectConfiguration)
        {
            var config = objectConfiguration as LoggingModuleConfig;
            if ( config == null)
            {
                throw new EnCorException(string.Format("Invalid config type for LoggingModule : {0}", objectConfiguration));
            }
            var appenders = new List<ILogAppender>();
            var factory = new LogAppenderFactory();
            foreach ( var appenderConfig in config.Appenders)
            {
                appenders.Add(factory.Build(appenderConfig, context));
            }
            return new LoggingModule(appenders);
        }

        #endregion
    }
}
