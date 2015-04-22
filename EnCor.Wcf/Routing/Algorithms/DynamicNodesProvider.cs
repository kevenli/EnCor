using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnCor.Wcf.Routing.Algorithms
{
    public class DynamicNodesProvider : StaticNodesProvider, INodesProvider
    {
        public DynamicNodesProvider(IEnumerable<NodeInfo> nodes):base(nodes, null)
        {

        }

        public void RegisterNode(NodeInfo nodeInfo)
        {
            lock (Nodes)
            {
                var existingNode = Nodes.FirstOrDefault(x => x.Action == nodeInfo.Action && x.Address == nodeInfo.Address);
                if (existingNode == null)
                {
                    Nodes.Add(nodeInfo);
                }
            }
        }

        public void UngisterNode(NodeInfo nodeInfo)
        {
            lock (Nodes)
            {
                var existingNode = Nodes.FirstOrDefault(x => x.Action == nodeInfo.Action && x.Address == nodeInfo.Address);
                if (existingNode != null)
                {
                    Nodes.Remove(existingNode);
                }
            }
        }
    }
}
