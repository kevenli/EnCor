using System;
using System.Collections.Generic;
using System.Text;
using EnCor.Configuration;
using EnCor.Logging;

namespace EnCor.ModuleLoader
{
    internal class DefaultModuleProvider : IDefaultModuleProvider
    {
        public IModuleConfig GetDefaultModuleConfig(Type moduleInterface)
        {
            if (moduleInterface == typeof(ILogging))
            {
                IModuleConfig result = new DefaultLoggingModuleConfig();
                return result;
            }
            return null;
        }
    }
}
