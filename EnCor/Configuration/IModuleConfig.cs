using System.Collections.Generic;
using EnCor.ModuleLoader;
using System;

namespace EnCor.Configuration
{
    public interface IModuleConfig
    {
        string ModuleName { get; }

        IEnumerable<IServiceConfig> Services { get; }

        IEnumerable<ModuleDependency> DependencyModules { get; }

        void Verify();

        IModuleConfig ActualConfig { get; }
    }
}
