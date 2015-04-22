using System;
using System.Configuration;

namespace EnCor.Configuration
{
    public class NameTypeConfigElement : NamedConfigurationElement
    {
        private const string ConfigType = "type";

        [ConfigurationProperty(ConfigType, IsRequired = true)]
        public virtual string TypeName
        {
            get
            {
                return (string)this[ConfigType];
            }
        }

        public Type Type
        {
            get
            {
                return Type.GetType(TypeName, true);
            }
        }
    }
}
