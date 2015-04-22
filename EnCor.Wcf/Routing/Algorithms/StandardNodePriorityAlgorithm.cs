using System.Collections.Generic;
using System.Linq;

namespace EnCor.Wcf.Routing.Algorithms
{
    public class StandardNodePriorityAlgorithm : INodePriorityAlgorithm
    {
        #region INodePriorityAlgorithm 成员

        public IList<NodeInfo> SortNodes(IList<NodeInfo> nodeList)
        {
            return new List<NodeInfo>(nodeList.OrderByDescending<NodeInfo, int>(x=> x.Rate / (x.Load+1)));
        }

        #endregion
    }
}
