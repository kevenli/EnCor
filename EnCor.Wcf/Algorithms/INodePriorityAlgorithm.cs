using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnCor.Wcf.Algorithms
{
    public interface INodePriorityAlgorithm
    {
        IList<NodeInfo> SortNodes(IList<NodeInfo> nodeList);
    }
}
