using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using EnCor.Configuration;

namespace EnCor.Wcf.Hosting
{
    public class ServiceConfig : NamedConfigurationElement
    {
        [ConfigurationProperty("contract")]
        public string Contract
        {
            get { return (string)this["contract"]; }
        }

        [ConfigurationProperty("serviceRef")]
        public string ServiceRef
        {
            get { return (string)this["serviceRef"]; }
        }

        [ConfigurationProperty("type")]
        public virtual string TypeName
        {
            get
            {
                return (string)this["type"];
            }
        }

        public Type Type
        {
            get { return Type.GetType(TypeName, true); }

        }

        [ConfigurationProperty("address", DefaultValue=null)]
        public string Address
        {
            get
            {
                return (string)this["address"];
            }
        }
    }
}
