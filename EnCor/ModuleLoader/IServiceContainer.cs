using System;
using System.Collections.Generic;
using System.Text;

namespace EnCor.ModuleLoader
{
    public interface IServiceContainer
    {
        void RegisterService(string moduleName, string serviceName, object instance, Type[] contracts);

        void RegisterService(string moduleName, string serviceName, object instance, Type contract);

        object GetService(Type contract);

        object GetService(string uniqueName);

        Type GetServiceInterface(object serviceInstance);

        object BuildUp(Type targetType);
    }
}
