using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnCor.Configuration;

namespace EnCor.Wcf.Routing
{
    public class NodeConfigCollection : NamedElementCollection<NodeConfig>
    {
        public NodeConfigCollection()
        {
            this.AddElementName = "node";
        }
    }
}
