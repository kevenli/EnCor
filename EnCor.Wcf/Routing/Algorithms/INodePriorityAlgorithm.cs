using System.Collections.Generic;

namespace EnCor.Wcf.Routing.Algorithms
{
    public interface INodePriorityAlgorithm
    {
        IList<NodeInfo> SortNodes(IList<NodeInfo> nodeList);
    }
}
