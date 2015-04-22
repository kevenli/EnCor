using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace EnCor.Wcf
{
    public class WcfClientMessageInspector : IClientMessageInspector, IEndpointBehavior
    {
        private HeaderBuilder _HeaderBuilder;

        public WcfClientMessageInspector(HeaderBuilder headerBuilder)
        {
            _HeaderBuilder = headerBuilder;
        }
        #region IClientMessageInspector Members

        public void AfterReceiveReply(ref System.ServiceModel.Channels.Message reply, object correlationState)
        {
            
        }

        public object BeforeSendRequest(ref System.ServiceModel.Channels.Message request, System.ServiceModel.IClientChannel channel)
        {

            foreach (MessageHeader header in _HeaderBuilder.BuildHeaders())
            {
                request.Headers.Add(header);
            }

            return null;
        }

        #endregion

        #region IEndpointBehavior Members

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            return;            
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(this);
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            throw new Exception("The EndpointBehaviorMessageInspector is not used in server applications.");
        }

        public void Validate(ServiceEndpoint endpoint)
        {
            return;
        }

        #endregion
    }
}
