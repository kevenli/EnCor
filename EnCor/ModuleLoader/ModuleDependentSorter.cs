using System;
using System.Collections.Generic;
using System.Text;
using EnCor.Configuration;

namespace EnCor.ModuleLoader
{
    public static class ModuleDependentSorter
    {
        public static void SortModuleByDependency(IModuleConfig currentModule, ref IList<IModuleConfig> sortedList, IEnumerable<IModuleConfig> allModuleConfigs)
        {
            if (sortedList.Contains(currentModule))
            {
                return;
            }
            foreach (ModuleDependency moduleDependency in currentModule.DependencyModules)
            {
                IModuleConfig dependentModule = null;
                bool dependentModuleFound = false;
                foreach (IModuleConfig moduleConfig in sortedList)
                {
                    if (moduleDependency.Match(moduleConfig))
                    {
                        dependentModuleFound = true;
                        break;
                    }
                }

                if (dependentModuleFound)
                {// if found a module match the one dependent, continue to check next dependent module
                    continue;
                }

                foreach (IModuleConfig moduleConfig in allModuleConfigs)
                {
                    if (moduleDependency.Match(moduleConfig))
                    {
                        dependentModule = moduleConfig;
                        break;
                    }
                }

                if (dependentModule == null)
                {
                    throw new EnCorException(string.Format("dependent module {0} not found", moduleDependency));
                }
                SortModuleByDependency(dependentModule, ref sortedList, allModuleConfigs);
            }
            sortedList.Add(currentModule);
        }
    }
}
