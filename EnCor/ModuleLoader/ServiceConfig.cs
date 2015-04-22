using System;
using System.Collections.Generic;
using System.Configuration;
using EnCor.Configuration;
using EnCor.ObjectBuilder;
using System.Xml;
using EnCor.Util;

namespace EnCor.ModuleLoader
{
    [Assembler(typeof(ServiceAssembler))]
    public class ServiceConfig : NameTypeConfigElement, IServiceConfig, IPolyMorphismConfig<IServiceConfig>
    {
        private const string ConfigDataType = "dataType";
        private const string ConfigInterface = "interface";
        private const string ConfigType = "type";
        private readonly List<Type> _interfaces = new List<Type>();

        private List<ServiceComponentConfig> _ComponentsConfig = new List<ServiceComponentConfig>();
        public IList<ServiceComponentConfig> ComponentsConfig
        {
            get
            {
                return _ComponentsConfig;
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

        [ConfigurationProperty(ConfigInterface)]
        public string Interface
        {
            get
            {
                return (string)this[ConfigInterface];
            }
        }

        public Type DataType
        {
            get
            {
                return Type.GetType(DataTypeName);
            }
        }

        //protected override bool OnDeserializeUnrecognizedElement(string elementName, System.Xml.XmlReader reader)
        //{
        //    if (ConfigComponent == elementName)
        //    {
        //        ServiceComponentConfig config = new ServiceComponentConfig();
        //        config.Deserialize(reader);
        //        if (config.Type == null)
        //        {
        //            throw new ServiceConfigException(string.Format("Cannot find the type {0}", config.TypeName));
        //        }

        //        if (config.DataType == null)
        //        {
        //            throw new ServiceConfigException(string.Format("Cannot find the type {0}", config.DataTypeName));
        //        }

        //        if (!(typeof(ServiceComponentConfig).IsAssignableFrom(config.DataType)))
        //        {
        //            throw new ServiceConfigException("Invalid config type");
        //        }

        //        ServiceComponentConfig specialconfig = (ServiceComponentConfig)Activator.CreateInstance(config.DataType);
        //        specialconfig.Deserialize(reader);


        //        _ComponentsConfig.Add(specialconfig);
        //        return true;
        //    }

        //    return base.OnDeserializeUnrecognizedElement(elementName, reader);
        //}

        private string _InnerContent;

        protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
        {
            _InnerContent = reader.ReadOuterXml();
            using (var innerReader = XmlHelper.BuildXmlReader(_InnerContent))
            {
                innerReader.Read();
                base.DeserializeElement(innerReader, serializeCollectionKey);
            }
        }

        private static ServiceConfig DeserializeServiceConfig(XmlReader reader)
        {
            Type configDataType = RetrieveConfigurationElementType(reader) ?? typeof(ServiceConfig);

            var specialconfig = (ServiceConfig)Activator.CreateInstance(configDataType);
            specialconfig.DeserializeElement(reader);
            return specialconfig;
        }

        private static Type RetrieveConfigurationElementType(XmlReader reader)
        {
            // type of dataType which describes what the configration sets, in "dataType" attribute
            Type configurationElementType = null;
            // type of the module element, in "type" attribute
            Type instanceType = null;
            if (reader.AttributeCount > 0)
            {
                // lookup attributes for Type and DataType
                for (bool go = reader.MoveToFirstAttribute(); go; go = reader.MoveToNextAttribute())
                {
                    // "dataType" attribute
                    if (reader.Name == ConfigDataType)
                    {
                        configurationElementType = Type.GetType(reader.Value);
                        if (configurationElementType == null)
                        {
                            throw new ConfigurationErrorsException(
                            string.Format("Invalid config type : {0}", reader.ReadOuterXml()));
                        }
                    }
                    // "type" attribute
                    if (reader.Name == ConfigType)
                    {
                        instanceType = Type.GetType(reader.Value);
                    }
                }
                reader.MoveToElement();
            }

            if (configurationElementType != null)
            {
                return configurationElementType;
            }

            if (instanceType != null)
            {
                return GetAssemblerConfigType(instanceType);
            }

            return null;
        }

        private static Type GetAssemblerConfigType(Type instanceType)
        {
            object[] attributes = instanceType.GetCustomAttributes(typeof(AssembleConfigAttribute), false);
            if (attributes.Length == 0)
            {
                return null;
            }
            var attribute = attributes[0] as AssembleConfigAttribute;
            if (attribute == null)
            {
                return null;
            }
            return attribute.ConfigType;
        }

        /// <summary>
        ///
        /// </summary>
        public void VerifyConfig()
        {
            if (!string.IsNullOrEmpty(Interface))
            {
                Type type = Type.GetType(Interface);
                _interfaces.Add(type);
            }

            //foreach ( ServiceContractConfig contractConfig in Contracts)
            //{
            //    _Interfaces.Add(contractConfig.Type);
            //}

            if (_interfaces.Count == 0)
            {
                _interfaces.Add(Type);
            }
        }

        #region IServiceConfig Members


        public Type ServiceType
        {
            get
            {
                return Type;
            }
        }

        public Type[] Contracts
        {
            get
            {
                return new [] { Type.GetType(Interface) };
            }
        }
        #endregion

        private IServiceConfig _ActualConfig;

        public IServiceConfig ActualConfig
        {
            get { return _ActualConfig; }
        }

        public void Verify()
        {
            XmlReader reader = XmlHelper.BuildXmlReader(_InnerContent);
            reader.Read();
            _ActualConfig = DeserializeServiceConfig(reader);
        }
    }
}
