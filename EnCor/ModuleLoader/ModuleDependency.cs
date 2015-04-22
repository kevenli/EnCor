using System;
using System.Collections.Generic;
using System.Text;
using EnCor.Configuration;

namespace EnCor.ModuleLoader
{
    public abstract class ModuleDependency
    {
        public abstract bool Match(IModuleConfig moduleConfig);

        public class ModuleNameDependency : ModuleDependency
        {
            private string _dependentModuleName;

            public ModuleNameDependency(string dependentModuleName)
            {
                _dependentModuleName = dependentModuleName;
            }

            public string DependentModuleName
            {
                get
                {
                    return _dependentModuleName;
                }
            }

            public override bool Match(IModuleConfig moduleConfig)
            {
                return moduleConfig.ModuleName == _dependentModuleName;
            }

            public override string ToString()
            {
                return "ModuleName : " + _dependentModuleName;
            }
        }

        //public class ModuleTypeDependency : ModuleDependency
        //{
        //    private Type _dependentModuleType;

        //    public ModuleTypeDependency(Type dependentModuleType)
        //    {
        //        _dependentModuleType = dependentModuleType;
        //    }

        //    public Type DependentModuleType
        //    {
        //        get
        //        {
        //            return _dependentModuleType;
        //        }
        //    }

        //    public override bool Match(IModuleConfig moduleConfig)
        //    {
        //        return _dependentModuleType.IsAssignableFrom(moduleConfig.ModuleType);
        //    }

        //    public override string ToString()
        //    {
        //        return "ModuleType : " + _dependentModuleType;
        //    }
        //}
    }
}
