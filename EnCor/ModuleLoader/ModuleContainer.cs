using System;
using System.Collections.Generic;
using System.Text;

namespace EnCor.ModuleLoader
{
    public class ModuleContainer
    {
        private readonly Dictionary<Type, IEnCorModule> _container = new Dictionary<Type, IEnCorModule>();
        private readonly Dictionary<string, IEnCorModule> _container2 = new Dictionary<string, IEnCorModule>();
        public IEnumerable<IEnCorModule> Modules
        {
            get
            {
                foreach (IEnCorModule Module in _container2.Values)
                {
                    yield return Module;
                }
            }
        }

        public void Add(string moduleName, IEnCorModule module)
        {
            _container2.Add(moduleName, module);
            foreach (Type moduleInterface in module.GetType().GetInterfaces())
            {
                if (typeof(IEnCorModule).IsAssignableFrom(moduleInterface) && moduleInterface != typeof(IEnCorModule))
                {
                    _container.Add(moduleInterface, module);
                }
            }
        }

        public T GetModule<T>()
            where T: IEnCorModule
        {
            return (T)_container[typeof(T)];
        }
    }
}
