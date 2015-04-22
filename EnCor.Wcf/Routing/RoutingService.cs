using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using EnCor.Hosting;
using EnCor.Wcf.Routing;
using System.ServiceModel.Description;
using System.Xml;
using EnCor.Logging;
using EnCor.Wcf.Routing.Algorithms;
using log4net;

namespace EnCor.Wcf.Hosting
{
    [ServiceContract(Namespace = "http://encor.codeplex.com/wcf/routing/")]
    public interface IRouterService
    {
        [OperationContract(Action = "*", ReplyAction = "*")]
        Message ProcessMessage(Message requestMessage);
    }


    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
        ConcurrencyMode = ConcurrencyMode.Multiple,
        MaxItemsInObjectGraph = int.MaxValue,
        UseSynchronizationContext = false)]
    public class RouterService : IRouterService
    {
        private INodePriorityAlgorithm _NodePriorityAlgorithm;
        private INodesProvider _NodesProvider;
        public RouterService(INodePriorityAlgorithm algorithm, INodesProvider nodesProvider)
        {
            _NodePriorityAlgorithm = algorithm;
            _NodesProvider = nodesProvider;
        }

        public Message ProcessMessage(Message requestMessage)
        {
            string messageAction = requestMessage.Headers.Action.Substring(0, requestMessage.Headers.Action.LastIndexOf("/"));
            Message responseMessage = null;
            IList<NodeInfo> nodes = _NodesProvider.GetNodes(messageAction);
            if (nodes.Count == 0)
            {
                Runtime.Logging.Warn(string.Format("Cannot find a node for action '{0}'", requestMessage.Headers.Action));
                return CreateErrorMessage("Router Error", "Cannot find a node address", requestMessage);
            }
            IList<NodeInfo> sortedNodes = _NodePriorityAlgorithm.SortNodes(nodes);
            MessageBuffer messageBuffer = requestMessage.CreateBufferedCopy(int.MaxValue);
            foreach (NodeInfo node in sortedNodes)
            {
                Message redirectedRequestMessage = messageBuffer.CreateMessage();

                try
                {
                    node.IncreaseLoad();
                    Binding binding = ConfigurationUtility.GetRouterBinding(new Uri(node.Address).Scheme);
                    ChannelFactory<IRouterService> factory = new ChannelFactory<IRouterService>(binding, node.Address);
                    factory.Endpoint.Behaviors.Add(new MustUnderstandBehavior(false));

                    IRouterService proxy = factory.CreateChannel();
                    using (proxy as IDisposable)
                    {
                        try
                        {
                            IClientChannel clientChannel = proxy as IClientChannel;
                            responseMessage = proxy.ProcessMessage(redirectedRequestMessage);
                            //if (responseMessage.IsFault) // if any fault, turn to next node
                            //{
                            //    Runtime.Logging.Info("RouterService.ProcessMessage(), responseMessage IsFault, MessageId=" + requestMessage.Headers.MessageId.ToString());
                            //    Console.WriteLine();
                            //    continue;
                            //}
                            _NodesProvider.RoutingSuccess(node);
                            return responseMessage;
                        }
                        catch (TimeoutException ex)
                        {
                            Runtime.Logging.Warn(string.Format("Service invoking failed : {0}", ex.Message), ex);
                            // Handle the timeout exception.
                            _NodesProvider.RoutingFailed(node, RoutingFailReason.Timeout);
                            factory.Abort();
                        }
                        catch (CommunicationException ex)
                        {
                            Runtime.Logging.Warn(string.Format("Service invoking failed : {0}", ex.Message), ex);
                            // Handle the communication exception.
                            _NodesProvider.RoutingFailed(node, RoutingFailReason.Unreachable);
                            factory.Abort();
                        }
                        catch (Exception ex)
                        {
                            Runtime.Logging.Warn(string.Format("Service invoking failed : {0}", ex.Message), ex);
                            _NodesProvider.RoutingFailed(node, RoutingFailReason.Unknown);
                            factory.Close();
                            continue;
                        }
                    }
                }
                finally
                {
                    node.DecreaseLoad();
                }
            }
            if (responseMessage == null)
            {
                Message errorMessage = CreateErrorMessage("Router Error", "Routing error: All nodes unreachable", requestMessage);
                return errorMessage;
                //MessageFault fault = MessageFault.CreateFault(new FaultCode("Failed"), new FaultReason("Routing error: All nodes unreachable"));
                
                //string action = requestMessage.Headers.Action;
                //Message errorMessage = Message.CreateMessage(MessageVersion.Default, fault, action + "Response");
                //errorMessage.Headers.RelatesTo = requestMessage.Headers.MessageId;
                //return errorMessage;
            }
            return responseMessage;
            //MessageBuffer messageBuffer = requestMessage.CreateBufferedCopy(int.MaxValue);
            //Message redirectedRequestMessage = messageBuffer.CreateMessage();
            //Message backupNextMessage = messageBuffer.CreateMessage();
            //Message backupExceptionMessage = messageBuffer.CreateMessage();

            //Binding binding = null;
            //EndpointAddress endpointAddress = null;
            //int invokeServiceHasCode;
            //UniqueId messageId = requestMessage.Headers.MessageId;
            //Console.WriteLine();
            //Console.WriteLine(String.Format("ProcessMessage At {0} TheardId={1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),System.Threading.Thread.CurrentThread.ManagedThreadId));
            //Console.WriteLine(String.Format("MessageId is {0}",  messageId.ToString()));

            //GetServiceEndpoint(requestMessage, out binding, out endpointAddress, out invokeServiceHasCode);
        }

        private Message CreateErrorMessage(string faultCode, string message, Message requestMessage)
        {
            string action = requestMessage.Headers.Action;
            UniqueId messageId = requestMessage.Headers.MessageId;
            MessageFault fault = MessageFault.CreateFault(new FaultCode(faultCode), new FaultReason(message));
            Message errorMessage = Message.CreateMessage(MessageVersion.Default, fault, action + "Response");
            errorMessage.Headers.RelatesTo = messageId;
            return errorMessage;
        }
    }
}
