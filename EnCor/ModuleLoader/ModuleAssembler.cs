using System;
using System.Collections.Generic;
using System.Text;
using EnCor.Configuration;
using EnCor.ObjectBuilder;
using EnCor.ServiceLocator;

namespace EnCor.ModuleLoader
{
    public class ModuleAssembler : IAssembler<IEnCorModule, IModuleConfig>
    {
        #region IAssembler<IEnCorModule,ModuleConfig> Members
        public IEnCorModule Assemble(IBuilderContext context, IModuleConfig objectConfiguration)
        {
            Module module = new Module();
            
            IServiceContainer container = context.GetExtension<IServiceContainer>();
            module.SetName(objectConfiguration.ModuleName);
            module.SetServiceContainer(container);
            ServiceFactory factory = new ServiceFactory();
            foreach (IServiceConfig serviceConfig in objectConfiguration.Services)
            {
                object service = factory.Build(serviceConfig, context);
                //container.RegisterService(objectConfiguration.ModuleName, serviceConfig.Name, service, service.GetType());
                RegisterService(container, objectConfiguration.ModuleName, serviceConfig.Name, service);
            }
            return module;
        }

        private void RegisterService(IServiceContainer container, string moduleName, string serviceName, object serviceInstance)
        {
            Type instanceType = serviceInstance.GetType();
            Type[] interfaces = instanceType.GetInterfaces();
            if (interfaces.Length > 0)
            {// if has any interface, register as interface
                foreach (var interfaceType in interfaces)
                {
                    container.RegisterService(moduleName, serviceName, serviceInstance, interfaceType);
                }
            }
            else
            {// if no interface, use the native type as contract
                container.RegisterService(moduleName, serviceName, serviceInstance, instanceType);
            }

        }

        #endregion
    }
}
