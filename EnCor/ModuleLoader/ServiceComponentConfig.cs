using System;
using System.Collections.Generic;
using System.Text;
using EnCor.Configuration;
using EnCor.ObjectBuilder;
using System.Configuration;
using System.Xml;

namespace EnCor.ModuleLoader
{
    [Assembler(typeof(ServiceComponentAssembler))]
    public class ServiceComponentConfig : PolymorphicConfig
    {
        private const string ConfigInterface = "interface";
        public void Deserialize(XmlReader reader)
        {
            base.DeserializeElement(reader, false);
        }

        [ConfigurationProperty(ConfigInterface)]
        public string InterfaceTypeName
        {
            get
            {
                return (string)this[ConfigInterface];
            }
        }

        public Type InterfaceType
        {
            get
            {
                string interfaceTypeName = InterfaceTypeName;
                if ( string.IsNullOrEmpty(interfaceTypeName))
                {
                    interfaceTypeName = this.TypeName;
                }

                return Type.GetType(interfaceTypeName);
            }
        }

        public virtual IAssembler<object, ServiceComponentConfig> GetAssembler()
        {
            AssemblerAttribute attribute =
            Attribute.GetCustomAttribute(this.GetType(), typeof(AssemblerAttribute)) as AssemblerAttribute;

            if (attribute == null)
            {
                throw new Exception("Cannot find assembler attribute.");
            }

            IAssembler<object, ServiceComponentConfig> assembler =
            (IAssembler<object, ServiceComponentConfig>)Activator.CreateInstance(attribute.AssemblerType);

            return assembler;
        }
    }
}
