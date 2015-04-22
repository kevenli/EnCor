using System;
using System.Configuration;
using System.Xml;
using EnCor.ObjectBuilder;

namespace EnCor.Configuration
{
    public class PolymorphicConfig : ConfigurationElement
    {
        private const string ConfigDataType = "dataType";

        private const string ConfigType = "type";

        private PolymorphicConfig _polymorphicInstance;

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

        public PolymorphicConfig PolymorphicInstance
        {
            get
            {
                return _polymorphicInstance;
            }
        }

        public void DeserializeElement(XmlReader reader)
        {
            base.DeserializeElement(reader, false);
        }

        //protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        //{
        //    return true;
        //}

        //protected override bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
        //{
        //    return true;
        //}

        protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
        {
            XmlReader copy = XmlReader.Create(reader, new XmlReaderSettings());
            base.DeserializeElement(reader, serializeCollectionKey);
            if (DataType != null)
            {
                if (GetType() == DataType)
                {
                    _polymorphicInstance = this;
                }
                else
                {
                    _polymorphicInstance = (PolymorphicConfig) Activator.CreateInstance(DataType);
                    _polymorphicInstance.DeserializeElement(copy);
                }
            }
            else
                if (Type != null)
                {
                    Type configActualType = GetConfigTypeFromType(Type);
                    _polymorphicInstance = (PolymorphicConfig)Activator.CreateInstance(configActualType);
                    _polymorphicInstance.DeserializeElement(copy);
                }
        }

        private Type GetConfigTypeFromType(Type type)
        {
            object[] attributes = type.GetCustomAttributes(typeof (AssembleConfigAttribute), true);
            if ( attributes.Length == 0)
            {
                return null;
            }
            var assembleConfigAttribute = attributes[0] as AssembleConfigAttribute;
            if (assembleConfigAttribute == null)
            {
                return null;
            }
            return assembleConfigAttribute.ConfigType;
        }
    }
}
