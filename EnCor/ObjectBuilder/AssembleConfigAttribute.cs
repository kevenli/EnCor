using System;
using System.Collections.Generic;
using System.Text;

namespace EnCor.ObjectBuilder
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class AssembleConfigAttribute : Attribute
    {
        private Type _ConfigType;

        public AssembleConfigAttribute(Type configType)
        {
            _ConfigType = configType;
        }

        public Type ConfigType
        {
            get
            {
                return _ConfigType;
            }
        }
    }
}
