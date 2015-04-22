using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using EnCor.Configuration;

namespace EnCor.Wcf.Routing
{
    public class NodeConfig : NamedConfigurationElement
    {
        const string STRActionUri = "actionUri";
        const string STRAddress = "address";
        const string STRRate = "rate";
        [ConfigurationProperty(STRActionUri, IsRequired=true)]
        public string ActionUri
        {
            get
            {
                return (string)this[STRActionUri];
            }
        }

        [ConfigurationProperty(STRAddress, IsRequired = true)]
        public string Address
        {
            get
            {
                return (string)this[STRAddress];
            }
        }

        [ConfigurationProperty(STRRate, IsRequired=false, DefaultValue=0)]
        public int Rate
        {
            get
            {
                return (int)this[STRRate];
            }
        }
    }
}
