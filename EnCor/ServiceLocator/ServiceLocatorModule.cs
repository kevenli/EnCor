using System;

namespace EnCor.ServiceLocator
{
    public class ServiceLocatorModule : Module, IServiceLocator
    {
        private readonly IContainer _container;

        public ServiceLocatorModule(IContainer container)
        {
            _container = container;
        }

        #region IServiceLocator Members

        public T GetService<T>()
        {
            return _container.GetInstance<T>();
        }

        public object GetService(string name)
        {
            string[] nameParts = name.Split('$');
            if ( nameParts.Length != 2)
            {
                throw new EnCorException(string.Format("Cannot get service '{0}', service name should be 'module$servicename'", name));
            }

            throw new NotImplementedException();
        }

        public void RegisterService(string moduleName, Type registerAs, object instance, string serviceName)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
