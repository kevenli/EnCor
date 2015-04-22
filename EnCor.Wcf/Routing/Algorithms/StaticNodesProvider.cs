using System;
using System.Collections.Generic;

namespace EnCor.Wcf.Routing.Algorithms
{
    public class StaticNodesProvider : INodesProvider
    {
        protected List<NodeInfo> Nodes;
        private AutoDelistConfig _autoDelistConfig;
        public StaticNodesProvider(IEnumerable<NodeInfo> nodes, AutoDelistConfig autoDelistConfig)
        {
            Nodes = new List<NodeInfo>(nodes);
            _autoDelistConfig = autoDelistConfig;
        }
        #region INodesProvider 成员

        public IList<NodeInfo> GetNodes(string action)
        {
            List<NodeInfo> result = new List<NodeInfo>();
            foreach (var node in Nodes)
            {
                if ( _autoDelistConfig != null  // current context has auto delist feature
                    && node.DelistedTime.HasValue // the node is delisted
                    && _autoDelistConfig.AutoResetPeroid > 0  // if the AutoResetPeriod equals or less than 0, the node will to be reset
                    && node.DelistedTime.Value.AddMinutes(_autoDelistConfig.AutoResetPeroid) < DateTime.Now // reach the reset time
                    )
                {
                    node.DelistedTime = null; // reset it.
                    node.FailTime = null;
                    node.FailCount = 0;
                    Runtime.Logging.Info(string.Format("Node {0} has been restarted, action '{1}', address '{2}'", node.Name, node.Action, node.Address));
                }

                if (node.Action == action && node.DelistedTime == null)
                {
                    result.Add(node);
                }
            }
            return result;
        }

        public void RoutingFailed(NodeInfo node, RoutingFailReason reason)
        {
            if ( node.FailTime != null
                && _autoDelistConfig != null 
                && _autoDelistConfig.FailTimesLimitation>0
                && _autoDelistConfig.FailPeroid >0 
                && node.FailTime.Value.AddMinutes(_autoDelistConfig.FailPeroid) < DateTime.Now
                )
            {
                node.FailTime = DateTime.Now; // pass the FailPeroid, reset fail counter
                node.FailCount = 0;
            }
                
            if ( node.FailTime == null)
            {
                node.FailTime = DateTime.Now; // first failure
            }
            
            node.FailCount++;
            if ( _autoDelistConfig != null 
                && _autoDelistConfig.FailPeroid > 0
                && _autoDelistConfig.FailTimesLimitation > 0
                && node.FailCount >= _autoDelistConfig.FailTimesLimitation)
            {
                node.DelistedTime = DateTime.Now; // delist, the node will not use in the next peroid
                Runtime.Logging.Info(string.Format("Node {0} has been delisted for {3} minutes, action '{1}', address '{2}'", node.Name, node.Action, node.Address, _autoDelistConfig.AutoResetPeroid));
            }
        }

        public void RoutingSuccess(NodeInfo node)
        {
            node.FailCount = 0;
            node.FailTime = null;
        }

        #endregion
    }
}
