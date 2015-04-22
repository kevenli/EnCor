using System;
using System.Collections.Generic;
using System.Text;
using EnCor.ModuleLoader;

namespace EnCor.ServiceLocator
{
    public interface IServiceLocator : IEnCorModule
    {
        T GetService<T>();

        object GetService(string serviceId);

        void RegisterService(string moduleName, Type registerAs, object instance, string serviceName);
    }
}
