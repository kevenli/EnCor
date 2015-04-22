using System;
using System.Collections.Generic;
using System.Text;
using EnCor.Configuration;
using EnCor.ObjectBuilder;
using EnCor.ModuleLoader;
using EnCor.Logging.Appenders;

namespace EnCor.Logging
{
    public class DefaultLoggingModuleConfig : ModuleConfig, IAssembler<IEnCorModule, IModuleConfig>
    {
        #region IAssembler<IEnCorModule,IModuleConfig> Members
        public IEnCorModule Assemble(IBuilderContext context, IModuleConfig objectConfiguration)
        {
            IList<ILogAppender> appenders = new List<ILogAppender>();
            appenders.Add(new ConsoleLogAppender());
            appenders.Add(new FileLogAppender("log/encor.log"));
            var logging = new LoggingModule(appenders);
            return logging;
        }

        #endregion
    }
}
