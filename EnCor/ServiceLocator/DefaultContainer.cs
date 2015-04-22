using System;
using System.Collections.Generic;
using System.Text;

namespace EnCor.ServiceLocator
{
    public class DefaultContainer : IContainer
    {
        #region IContainer Members
        public void RegisterService<TService>(TService service)
        {
            throw new NotImplementedException();
        }

        public void RegisterService(Type t, object instance)
        {
            throw new NotImplementedException();
        }

        public TService GetService<TService>()
        {
            throw new NotImplementedException();
        }

        public object GetService(Type t)
        {
            throw new NotImplementedException();
        }

        public IContainer CreateChildContainer()
        {
            throw new NotImplementedException();
        }

        public void RegisterPropertyInjection(Type targetType, string propertyName)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IContainer Members


        public TInterface GetInstance<TInterface>()
        {
            throw new NotImplementedException();
        }

        public object GetInstance(Type t)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
