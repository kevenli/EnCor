using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using EnCor.Configuration;

namespace EnCor.Wcf.NodeHosting
{
    public class FacadeServiceConfig : NameTypeConfigElement
    {
        const string STR_Url = "url";
        [ConfigurationProperty(STR_Url)]
        public string Url
        {
            get
            {
                return (string)this[STR_Url];
            }
        }


    }
}
