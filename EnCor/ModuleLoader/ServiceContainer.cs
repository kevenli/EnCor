using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace EnCor.ModuleLoader
{
    public class ServiceContainer : MarshalByRefObject, IServiceContainer
    {
        private Dictionary<Type, object> _typeContainer = new Dictionary<Type, object>();

        private Dictionary<string, Dictionary<string, object>> _moduleServicesContainer =
            new Dictionary<string, Dictionary<string, object>>();
        #region IServiceContainer Members

        public void RegisterService(string moduleName, string serviceName, object instance, Type[] contracts)
        {
            if (!_moduleServicesContainer.ContainsKey(moduleName))
            {
                _moduleServicesContainer.Add(moduleName, new Dictionary<string, object>());
            }
            _moduleServicesContainer[moduleName].Add(serviceName, instance);

            foreach ( var instanceType in contracts)
            {
                _typeContainer.Add(instanceType, instance);
            }
        }

        public void RegisterService(string moduleName, string serviceName, object instance, Type contract)
        {
            if (!_moduleServicesContainer.ContainsKey(moduleName))
            {
                _moduleServicesContainer.Add(moduleName, new Dictionary<string, object>());
            }
            _moduleServicesContainer[moduleName].Add(serviceName, instance);
            _typeContainer.Add(contract, instance);
        }

        public object GetService(Type contract)
        {
            if (_typeContainer.ContainsKey(contract))
            {
                return _typeContainer[contract];
            }
            return null;
        }

        public object GetService(string uniqueName)
        {
            string[] nameParts = uniqueName.Split('$');
            if (nameParts.Length != 2)
            {
                throw new EnCorException(string.Format("Cannot get service '{0}', service name should be 'module$servicename'", uniqueName));
            }
            string moduleName = nameParts[0];
            string serviceName = nameParts[1];

            return _moduleServicesContainer[moduleName][serviceName];
        }

        public Type GetServiceInterface(object service)
        {
            foreach (var key in _typeContainer)
            {
                if (key.Value == service)
                {
                    return key.Key;
                }
            }
            return null;
        }

        #endregion


        public object BuildUp(Type targetType)
        {
            ConstructorInfo construstor = GetLongestConstructor(targetType);
            List<object> parameters = new List<object>();
            foreach( var parameterInfo in construstor.GetParameters() )
            {
                object parameter = this.GetService(parameterInfo.ParameterType);
                if ( parameter == null )
                {
                    throw new Exception(string.Format("Cannot retrieve type {0}", parameterInfo.ParameterType));
                }
                parameters.Add(parameter);
            }
            return construstor.Invoke(parameters.ToArray());
        }

        private static ConstructorInfo GetLongestConstructor(Type targetType)
        {
            var concs = targetType.GetConstructors();
            if (concs.Length == 0)
            {
                throw new Exception(string.Format("Type {0} has no contructor", targetType));
            }

            int length = 0;
            ConstructorInfo ret = concs[0];
            for (int i = 1; i < concs.Length; i++ )
            {
                ConstructorInfo current = concs[i];
                if (current.GetParameters().Length > length)
                {
                    length = current.GetParameters().Length;
                    ret = current;
                }
            }

            return ret;
        }
    }
}
