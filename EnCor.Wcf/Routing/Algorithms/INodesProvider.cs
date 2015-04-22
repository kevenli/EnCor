using System.Collections.Generic;

namespace EnCor.Wcf.Routing.Algorithms
{
    public interface INodesProvider
    {
        IList<NodeInfo> GetNodes(string action);

        void RoutingFailed(NodeInfo node, RoutingFailReason reason);

        void RoutingSuccess(NodeInfo node);

        //IList<NodeInfo> GetNodes(string uriScheme);
    }
}
