using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using EnCor.ObjectBuilder;

namespace EnCor.Configuration
{
    public abstract class PolymorphicConfigurationElementCollection<T> : NamedElementCollection<T>
        where T: NamedConfigurationElement, new()
    {
        private const string ConfigType = "type";
        private const string ConfigDataType = "dataType";
        private Dictionary<string, Type> _configurationElementTypeMapping;
        private T _currentElement;


        /// <summary>
        /// Resets the internal state of the <see cref="ConfigurationElement"/> object, including the locks and the properties collections.
        /// </summary>
        /// <param name="parentElement">The parent node of the configuration element.</param>
        protected override void Reset(ConfigurationElement parentElement)
        {
            CreateTypesMap((PolymorphicConfigurationElementCollection<T>)parentElement);

            base.Reset(parentElement);

            ReleaseTypesMap();
        }

        /// <summary>
        /// Called when an unknown element is encountered while deserializing the <see cref="ConfigurationElement"/> object.
        /// </summary>
        /// <param name="elementName">The name of the element.</param>
        /// <param name="reader">The <see cref="XmlReader"/> used to deserialize the element.</param>
        /// <returns>  <see langword="true"/> if the element was handled; otherwise, <see langword="false"/>.</returns>
        protected override bool OnDeserializeUnrecognizedElement(string elementName, XmlReader reader)
        {
            if (AddElementName.Equals(elementName))
            {
                Type configurationElementType = RetrieveConfigurationElementType(reader);
                if (configurationElementType == null)
                {
                    throw new ConfigurationErrorsException(string.Format("Cannot find polymorphic config type for configElement: {0}", elementName));
                }
                _currentElement = (T)Activator.CreateInstance(configurationElementType);
                _currentElement.DeserializeElement(reader);
                Add(_currentElement);
                return true;
            }
            return base.OnDeserializeUnrecognizedElement(elementName, reader);
        }

        /// <summary>
        /// When overriden in a class, get the configuration object for each <see>
        /// <cref>NameTypeConfigurationElement</cref>
        /// </see> object in the collection.
        /// </summary>
        /// <param name="reader">The <see cref="XmlReader"/> that is deserializing the element.</param>
        protected virtual Type RetrieveConfigurationElementType(XmlReader reader)
        {
            // type of dataType which describes what the configration sets.
            Type configurationElementType = null;
            // type of the element 
            Type instanceType = null;

            if (reader.AttributeCount > 0)
            {
                // expect the first attribute to be the name
                for (bool go = reader.MoveToFirstAttribute(); go; go = reader.MoveToNextAttribute())
                {
                    // type attribute
                    if (reader.Name == ConfigDataType)
                    {
                        configurationElementType = Type.GetType(reader.Value);
                        if (configurationElementType == null)
                        {
                            throw new ConfigurationErrorsException(
                            string.Format("Invalid config type : {0}", reader.ReadOuterXml()));
                        }
                    }
                    if (reader.Name == ConfigType)
                    {
                        instanceType = Type.GetType(reader.Value);
                        if (instanceType == null)
                        {
                            throw new ConfigurationErrorsException(
                            string.Format("Invalid config type : {0}", reader.ReadOuterXml()));
                        }
                    }
                    // dataType attribute

                }
                // cover the traces
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

        private Type GetAssemblerConfigType(Type instanceType)
        {
            object[] attributes = instanceType.GetCustomAttributes(typeof(AssembleConfigAttribute), true);
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
        /// Creates a new <see cref="ConfigurationElement"/>. 
        /// </summary>
        /// <returns>A new <see cref="ConfigurationElement"/>.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            // create a new instance of the type we need...
            if (_currentElement != null)
            {
                return _currentElement;
            }
            return new T();
        }

        /// <summary>
        /// Creates a new named <see cref="ConfigurationElement"/>.
        /// </summary>
        /// <param name="elementName">The name of the element to create.</param>
        /// <returns>A new <see cref="ConfigurationElement"/>.</returns>
        protected override ConfigurationElement CreateNewElement(string elementName)
        {
            if (_configurationElementTypeMapping != null)
            {
                Type configurationElementType = _configurationElementTypeMapping[elementName];
                if (configurationElementType != null)
                {
                    return (ConfigurationElement)Activator.CreateInstance(configurationElementType);
                }
            }
            return base.CreateNewElement(elementName);
        }

        /// <summary>
        /// Reverses the effect of merging configuration information from different levels of the configuration hierarchy.
        /// </summary>
        /// <param name="sourceElement">A <see cref="ConfigurationElement"/> object at the current level containing a merged view of the properties.</param>
        /// <param name="parentElement">The parent <see cref="ConfigurationElement"/> object of the current element, or a <see langword="null"/> reference (Nothing in Visual Basic) if this is the top level.</param>
        ////// <param name="saveMode">One of the <see cref="ConfigurationSaveMode"/> values.</param>
        protected override void Unmerge(ConfigurationElement sourceElement, ConfigurationElement parentElement, ConfigurationSaveMode saveMode)
        {
            CreateTypesMap((PolymorphicConfigurationElementCollection<T>)sourceElement);
            base.Unmerge(sourceElement, parentElement, saveMode);
            ReleaseTypesMap();
        }

        private void CreateTypesMap(PolymorphicConfigurationElementCollection<T> sourceCollection)
        {
            _configurationElementTypeMapping = new Dictionary<string, Type>(sourceCollection.Count);

            foreach (T configurationElementSettings in sourceCollection)
            {
                _configurationElementTypeMapping.Add(configurationElementSettings.Name, configurationElementSettings.GetType());
            }
        }

        private void ReleaseTypesMap()
        {
            _configurationElementTypeMapping = null;
        }

        public void DeserializeElement(XmlReader reader)
        {
            base.DeserializeElement(reader, false);
        }
    }
}
