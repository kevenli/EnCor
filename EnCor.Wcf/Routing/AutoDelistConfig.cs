using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace EnCor.Wcf.Routing
{
    public class AutoDelistConfig : ConfigurationElement
    {
        [ConfigurationProperty("failTimesLimitation", DefaultValue = 0)]
        public int FailTimesLimitation 
        { 
            get { return (int)this["failTimesLimitation"]; }
        }

        [ConfigurationProperty("failPeroid", DefaultValue = 0)]
        public int FailPeroid
        {
            get { return (int) this["failPeroid"]; }
        }

        [ConfigurationProperty("autoResetPeroid", DefaultValue = 0)]
        public int AutoResetPeroid
        {
            get { return (int) this["autoResetPeroid"]; }
        }
    }
}
