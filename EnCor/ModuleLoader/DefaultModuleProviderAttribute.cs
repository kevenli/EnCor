using System;
using System.Collections.Generic;
using System.Text;

namespace EnCor.ModuleLoader
{
    internal class DefaultModuleProviderAttribute : Attribute
    {
        private Type _defaultModuleProviderType;
        public DefaultModuleProviderAttribute(Type defaultModuleProviderType)
        {
            _defaultModuleProviderType = defaultModuleProviderType;
        }

        public Type DefaultModuleProviderType
        {
            get
            {
                return _defaultModuleProviderType;
            }
        }
    }
}
