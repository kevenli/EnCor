using System;
using System.Collections.Generic;
using System.Text;
using EnCor.Configuration;
using EnCor.ObjectBuilder;
using System.Collections;

namespace EnCor.ModuleLoader
{
    public class ModuleInitializer : MarshalByRefObject
    {
        private IModuleConfig _ModuleConfig;
        private IModuleConfig _ActualModuleConfig;
        private string _ModuleName;
        private IEnCorModule _Module;
        public void VefiryConfig()
        {
            _ModuleConfig.Verify();
            _ActualModuleConfig = _ModuleConfig.ActualConfig;
        }

        public void SetModuleConfig(IModuleConfig moduleConfig)
        {
            _ModuleName = moduleConfig.ModuleName;
            _ModuleConfig = moduleConfig;
        }

        public void Init(IServiceContainer serviceContainer)
        {
            Runtime.InitInSubDomain(serviceContainer);

            ModuleFactory factory = new ModuleFactory();
            var moduleBuildContext = new BuilderContext();
            moduleBuildContext.Register<IServiceContainer>(Runtime.GetServiceContainer());

            _Module = factory.Build(_ActualModuleConfig, moduleBuildContext);
        }

        public void Start()
        {
            _Module.Start();
        }

        internal void Stop()
        {
            _Module.Stop();
        }
    }
}
