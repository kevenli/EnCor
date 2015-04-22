using System;
using System.Collections.Generic;
using System.Text;

namespace EnCor.ModuleLoader
{
    public class ModuleDependentCollection : System.Configuration.ConfigurationElementCollection
    {
        public ModuleDependentCollection()
        {
            this.AddElementName = "module";
        }

        protected override System.Configuration.ConfigurationElement CreateNewElement()
        {
            return new ModuleDependentElement();
        }

        protected override object GetElementKey(System.Configuration.ConfigurationElement element)
        {
            return ((ModuleDependentElement)element).ModuleName;
        }
    }
}
