using System;
using System.Collections.Generic;
using System.Text;

namespace EnCor.ServiceLocator
{
    public interface IContainer
    {
        void RegisterService<TInterface>(TInterface instance);

        void RegisterService(Type t, object instance);

        TInterface GetInstance<TInterface>();

        object GetInstance(Type t);

        IContainer CreateChildContainer();
    }
}
