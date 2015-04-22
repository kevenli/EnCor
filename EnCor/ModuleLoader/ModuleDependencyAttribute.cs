using System;
using System.Collections.Generic;
using System.Text;

namespace EnCor.ModuleLoader
{
    public class ModuleDependencyAttribute : Attribute
    {
        private Type _dependencyModuleType;

        public ModuleDependencyAttribute(Type dependencyModuleType)
        {
            _dependencyModuleType = dependencyModuleType;
        }

        public Type DependencyModuleType
        {
            get
            {
                return _dependencyModuleType;
            }
        }
    }
}
