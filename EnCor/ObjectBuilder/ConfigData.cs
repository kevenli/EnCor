using System;
using System.Configuration;
using System.Xml;
using EnCor.Configuration;

namespace EnCor.ObjectBuilder
{
    public class ConfigData : NameTypeConfigElement
    {
        private const string ConfigDataType = "dataType";
        public void DeserializeElement(XmlReader reader)
        {
            base.DeserializeElement(reader, false);
        }

        [ConfigurationProperty(ConfigDataType)]
        public string DataTypeName
        {
            get
            {
                return (string)this[ConfigDataType];
            }
        }

        public Type DataType
        {
            get
            {
                return Type.GetType(DataTypeName);
            }
        }
    }
}
