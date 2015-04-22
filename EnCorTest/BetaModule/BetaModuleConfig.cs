using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnCor.Config;
using System.Configuration;

namespace EnCorTest.BetaModule
{
    public class BetaModuleConfig : ModuleConfig
    {
        [ConfigurationProperty("moduleStringParameter", DefaultValue="value")]
        public string StringParameter
        {
            get
            {
                return (string)this["moduleStringParameter"];
            }
        }
        
        [ConfigurationProperty("moduleNumberParameter", DefaultValue=123)]
        public int NumberParameter
        {
            get
            {
                return (int)this["moduleNumberParameter"];
            }
        }
    }
}
