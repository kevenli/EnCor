using System;
using System.Collections.Generic;
using System.Text;
using EnCor.Configuration;
using System.Configuration;

namespace EnCor.ModuleLoader
{
    public class ServiceConfigCollection : ConfigurationElementCollection
    {
        private const string ConfigService = "service";
        public ServiceConfigCollection()
        {
            AddElementName = ConfigService;
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceConfig();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceConfig)element).Name;
        }
    }
}
