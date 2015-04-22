using System;
using System.Collections.Generic;
using System.Text;

namespace EnCor.ModuleLoader
{
    [Serializable]
    public class ModuleInfo
    {
        private string moduleName;
        private AppDomain moduleDomain;
        private ModuleInitializer moduleInitializer;

        public ModuleInfo(string moduleName, AppDomain moduleDomain, ModuleInitializer moduleInitializer)
        {
            // TODO: Complete member initialization
            this.moduleName = moduleName;
            this.moduleDomain = moduleDomain;
            this.moduleInitializer = moduleInitializer;
        }

        public string ModuleName
        {
            get
            {
                return moduleName;
            }
        }

        public ModuleInitializer Initializer
        {
            get
            {
                return moduleInitializer;
            }
        }
    }
}
