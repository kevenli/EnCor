using System;
using System.Collections.Generic;
using System.Configuration;
using EnCor.Configuration;
using EnCor.ObjectBuilder;
using System.Xml;
using System.IO;
using EnCor.Util;

namespace EnCor.ModuleLoader
{
    [Assembler(typeof(ModuleAssembler))]
    public class ModuleConfig : ConfigurationElement, IModuleConfig, IPolyMorphismConfig<IModuleConfig>
    {
        private const string ConfigType = "type";
        private const string ConfigDataType = "dataType";
        private const string ConfigServices = "services";
        private const string ConfigDependents = "dependents";
        private const string ConfigModuleName = "name";
        private string _moduleName;
        private string _InnerContent;
        private IList<IServiceConfig> _ServinceConfigs = new List<IServiceConfig>();

        private ServiceConfigCollection _collection = new ServiceConfigCollection();

        [ConfigurationProperty(ConfigType, DefaultValue = null)]
        public string TypeName
        {
            get
            {
                return (string)this[ConfigType];
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

        [ConfigurationProperty(ConfigDependents)]
        public ModuleDependentCollection DependModuleConfig
        {
            get
            {
                return (ModuleDependentCollection)this[ConfigDependents];
            }
        }

        [ConfigurationProperty(ConfigServices)]
        public ServiceConfigCollection ServiceCollection
        {
            get
            {
                return (ServiceConfigCollection)this[ConfigServices];
            }
        }

        public string ModuleName
        {
            get;
            internal set;
        }

        public Type DataType
        {
            get
            {
                return Type.GetType(DataTypeName);
            }
        }

        public void DeserializeElement(System.Xml.XmlReader reader)
        {
            _moduleName = reader.LocalName;
            DeserializeElement(reader, false);
        }

        protected override bool OnDeserializeUnrecognizedElement(string elementName, System.Xml.XmlReader reader)
        {
            return true;
        }

        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            return true;
        }

        protected override void DeserializeElement(XmlReader reader, bool serializeCollectionKey)
        {
            _InnerContent = reader.ReadOuterXml();

            NameTable nt = new NameTable();
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(nt);

            // Create the XmlParserContext.
            XmlParserContext context = new XmlParserContext(null, nsmgr, null, XmlSpace.None);
            using (var innerReader = new XmlTextReader(_InnerContent, XmlNodeType.Document, context))
            {
                innerReader.Read();
                base.DeserializeElement(innerReader, serializeCollectionKey);
            }
        }

        #region IModuleConfig Members

        IEnumerable<IServiceConfig> IModuleConfig.Services
        {
            get
            {
                foreach (var config in this._ServinceConfigs)
                {
                    yield return config;
                }
            }
        }

        #endregion

        #region IModuleConfig Members

        public IEnumerable<ModuleDependency> DependencyModules
        {
            get
            {
                List<ModuleDependency> result = new List<ModuleDependency>();
                //foreach (object attributes in this.ModuleType.GetCustomAttributes(typeof(ModuleDependencyAttribute), true))
                //{
                //    result.Add(new ModuleDependency.ModuleTypeDependency(((ModuleDependencyAttribute)attributes).DependencyModuleType));
                //}

                foreach (ModuleDependentElement element in DependModuleConfig)
                {
                    result.Add(new ModuleDependency.ModuleNameDependency(element.ModuleName));
                }
                return result;
            }
        }

        #endregion

        public static IModuleConfig ParseConfig(string filePath)
        {
            filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filePath);
            string moduleName = Path.GetFileName(Path.GetDirectoryName(filePath));
            if (File.Exists(filePath))
            {
                using (XmlTextReader reader = new XmlTextReader(filePath))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {
                            break;
                        }
                    }

                    //reader.ReadStartElement("module");
                    //reader.ReadStartElement("enCor");
                    //reader.ReadStartElement("module");
                    //reader.re
                    ModuleConfig moduleConfig = new ModuleConfig();
                    moduleConfig.DeserializeElement(reader);
                    moduleConfig.ModuleName = moduleName;
                    return moduleConfig;
                }
            }

            throw new Exception(string.Format("Cannot find module.config in path {0}.", filePath));
        }

        public static IModuleConfig ParseConfig(XmlReader reader, string moduleName)
        {
            var moduleConfig = DeserializeModuleConfig(reader);
            moduleConfig.ModuleName = moduleName;
            return moduleConfig;
        }

        private static ModuleConfig DeserializeModuleConfig(XmlReader reader)
        {
            Type configDataType = RetrieveConfigurationElementType(reader) ?? typeof(ModuleConfig);

            var specialconfig = (ModuleConfig)Activator.CreateInstance(configDataType);
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
                        if (instanceType == null)
                        {
                            throw new ConfigurationErrorsException(
                            string.Format("Invalid config type : {0}", reader.ReadOuterXml()));
                        }
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


        public void Verify()
        {
            XmlReader reader = XmlHelper.BuildXmlReader(_InnerContent);
            reader.Read();
            _ActualConfig = DeserializeModuleConfig(reader);
            ModuleConfig actualModuleConfig = _ActualConfig as ModuleConfig;
            if (actualModuleConfig != null)
            {
                actualModuleConfig.ModuleName = this.ModuleName;
            }

            foreach (ServiceConfig serviceConfig in actualModuleConfig.ServiceCollection)
            {
                serviceConfig.Verify();
                actualModuleConfig._ServinceConfigs.Add(serviceConfig.ActualConfig);
            }
        }

        private IModuleConfig _ActualConfig;

        public IModuleConfig ActualConfig
        {
            get { return _ActualConfig; }
        }
    }
}
