using System;
using System.Collections.Generic;
using System.Text;
using EnCor.Configuration;
using EnCor.Wcf.NodeHosting;

namespace EnCor.Hosting
{
    public class FacadeServiceConfigCollection : NamedElementCollection<FacadeServiceConfig>
    {
        const string STR_Add = "add";
        public FacadeServiceConfigCollection()
        {
            this.AddElementName = STR_Add;
        }
    }
}
