using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;
using System.IO;
using System.Xml;
using EnCor.Logging;
using EnCor.Wcf.Routing.Algorithms;

namespace EnCor.Wcf.Routing
{
    public delegate void ReProcessMessage(Message requestMessage);

    [ServiceContract(Namespace = "EnCor.Wcf.Routing.IDuplexRouterService", SessionMode = SessionMode.Required, CallbackContract = typeof(IDuplexRouterCallback))]
    public interface IDuplexRouterService
    {
        [OperationContract(IsOneWay = true, Action = "*")]
        void ProcessMessage(Message requestMessage);

    }

    [ServiceContract(Namespace = "EnCor.Wcf.Routing.IDuplexRouterCallback", SessionMode = SessionMode.Allowed)]
    public interface IDuplexRouterCallback
    {
        [OperationContract(IsOneWay = true, Action = "*")]
        void ProcessMessage(Message requestMessage);

    }


    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall, ConcurrencyMode = ConcurrencyMode.Multiple, AddressFilterMode = AddressFilterMode.Any, ValidateMustUnderstand = false)]
    public class DuplexRouterService : IDuplexRouterService, IDisposable, IDuplexRouterCallback
    {
        object m_duplexSessionLock = new object();
        IDuplexRouterService m_duplexSession;
        IDuplexRouterCallback callback;

        private INodesProvider _NodesProvider;
        private INodePriorityAlgorithm _Algorithm;
        private IEnumerator<NodeInfo> _NodeEnumrator;
        private Message lastResponse;

        public DuplexRouterService(INodePriorityAlgorithm algorithm, INodesProvider nodesProvider)
        {
            _Algorithm = algorithm;
            _NodesProvider = nodesProvider;
        }

        public void ProcessMessage(Message requestMessage)
        {
            if (callback == null)
            {
                lock (this.m_duplexSessionLock)
                {
                    if (this.m_duplexSession == null)
                    {
                        callback =
                          OperationContext.Current.GetCallbackChannel
                          <IDuplexRouterCallback>();

                    }
                }
            }
            if (_NodeEnumrator == null)
            {
                string messageAction = requestMessage.Headers.Action.Substring(0, requestMessage.Headers.Action.LastIndexOf("/"));
                IList<NodeInfo> nodes = _NodesProvider.GetNodes(messageAction);
                IList<NodeInfo> sorted = _Algorithm.SortNodes(nodes);
                _NodeEnumrator = sorted.GetEnumerator();
            }
            while (_NodeEnumrator.MoveNext())
            {
                Binding binding = new CustomBinding(new BinaryMessageEncodingBindingElement(), new TcpTransportBindingElement { ManualAddressing = true });
                DuplexChannelFactory<IDuplexRouterService> factory =
                  new DuplexChannelFactory<IDuplexRouterService>
                  (new InstanceContext(null,
                  this), binding, new EndpointAddress(_NodeEnumrator.Current.Address));
                factory.Endpoint.Behaviors.Add(new MustUnderstandBehavior(false));
                this.m_duplexSession = factory.CreateChannel();


                try
                {
                    this.m_duplexSession.ProcessMessage(requestMessage);
                    // if the node can process the message, return and wait for result.
                    return;
                }
                catch (EndpointNotFoundException ex)
                {
                    Runtime.Logging.Warn("路由错误", ex);
                    continue;
                }
                catch (CommunicationException ex)
                {
                    Runtime.Logging.Warn("路由错误", ex);
                    continue;
                }
            }

            if (lastResponse != null)
            {
                callback.ProcessMessage(lastResponse);
                return;
            }

            // if no more nodes, return immediately
            MessageFault fault = MessageFault.CreateFault(new FaultCode("NoEndpointFound"), new FaultReason("Cannot find endpoint"));
            string action = requestMessage.Headers.Action;
            Message errorMessage = Message.CreateMessage(MessageVersion.Default, fault, action + "Response");
            errorMessage.Headers.RelatesTo=requestMessage.Headers.MessageId;
            callback.ProcessMessage(errorMessage);
           
            return;
        }
        public void Dispose()
        {
            if (this.m_duplexSession != null)
            {
                try
                {
                    ICommunicationObject obj = this.m_duplexSession as
                      ICommunicationObject;
                    if (obj.State == CommunicationState.Faulted)
                        obj.Abort();
                    else
                        obj.Close();
                }
                catch { }
            }
        }

        #region IDuplexRouterCallback 成员

        void IDuplexRouterCallback.ProcessMessage(Message requestMessage)
        {
            lastResponse = requestMessage;
            if (requestMessage.IsFault)
            {
                this.ProcessMessage(requestMessage);
                return;
            }
            this.callback.ProcessMessage(requestMessage);
        }

        #endregion
    }

    public class DuplexRouterCallback : IDuplexRouterCallback
    {

        private IDuplexRouterCallback m_clientCallback;

        public DuplexRouterCallback(IDuplexRouterCallback clientCallback)
        {
            m_clientCallback = clientCallback;
        }

        public void ProcessMessage(Message requestMessage)
        {
            this.m_clientCallback.ProcessMessage(requestMessage);
        }
    }

}

