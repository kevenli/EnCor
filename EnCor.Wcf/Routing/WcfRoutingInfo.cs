using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Channels;

namespace EnCor.Wcf.Hosting
{
    public class WcfRoutingInfo
    {
        public string ServiceName
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public string Action
        {
            get;
            set;
        }

        public Binding Binding
        {
            get;
            set;
        }
    }
}
