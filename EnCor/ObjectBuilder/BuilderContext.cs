using System;
using System.Collections.Generic;
using System.Text;

namespace EnCor.ObjectBuilder
{
    public class BuilderContext : IBuilderContext
    {
        private Dictionary<Type, object> _typeRegister = new Dictionary<Type, object>();
        private Dictionary<string, object> _nameRegister = new Dictionary<string, object>();

        public void Register<T>(T tobject)
        {
            _typeRegister.Add(typeof(T), tobject);
        }

        public void Register<T>(string name, T tobject)
        {
            _nameRegister.Add(name, tobject);
        }

        #region IBuilderContext Members

        public T GetExtension<T>()
        {
            return (T) _typeRegister[typeof (T)];
        }

        public T GetExtension<T>(string name)
        {
            return (T) _nameRegister[name];
        }

        #endregion
    }
}
