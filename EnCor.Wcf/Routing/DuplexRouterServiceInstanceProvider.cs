using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Description;
using EnCor.Wcf.Routing.Algorithms;

namespace EnCor.Wcf.Routing
{
    class DuplexRouterServiceInstanceProvider: IInstanceProvider, IServiceBehavior 
    {

        private INodePriorityAlgorithm _NodePriorityAlgorithm;
        private INodesProvider _NodesProvider;

        public DuplexRouterServiceInstanceProvider(INodePriorityAlgorithm algorithm, INodesProvider nodesProvider)
        {
            _NodePriorityAlgorithm = algorithm;
            _NodesProvider = nodesProvider;
        }

        #region IInstanceProvider 成员



        public object GetInstance(System.ServiceModel.InstanceContext instanceContext, System.ServiceModel.Channels.Message message)
        {
            return new DuplexRouterService(_NodePriorityAlgorithm, _NodesProvider);
        }

        public object GetInstance(System.ServiceModel.InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        public void ReleaseInstance(System.ServiceModel.InstanceContext instanceContext, object instance)
        {
            if (instance is DuplexRouterService)
            {
                ((DuplexRouterService)instance).Dispose();
            }
            return;
        }

        #endregion

        #region IServiceBehavior 成员

        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
            
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcherBase channelDispatcherBase in serviceHostBase.ChannelDispatchers)
            {
                ChannelDispatcher channelDispatcher = channelDispatcherBase as ChannelDispatcher;
                if (channelDispatcher != null)
                {
                    foreach (EndpointDispatcher endpoint in channelDispatcher.Endpoints)
                    {
                        endpoint.DispatchRuntime.InstanceProvider = this;
                    }
                }
            }
        }

        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            
        }

        #endregion
    }
}
