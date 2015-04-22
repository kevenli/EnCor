using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace EnCor.ModuleLoader
{
    public class ModuleDependentElement : ConfigurationElement
    {
        const string StrModule = "module";
        const string StrName = "name";

        [ConfigurationProperty(StrName, IsRequired = true)]
        public string ModuleName
        {
            get
            {
                return (string)this[StrName];
            }
        }
    }
}
