using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace EnCor.Wcf
{
    public static class ClientFactory
    {
        public static T CreateClient<T>(string serviceEndpoint, BindingType bindingType)
        {
            Binding binding = GetClientBinding(bindingType);
            ChannelFactory<T> factory = new ChannelFactory<T>(binding, serviceEndpoint);
            var operations = factory.Endpoint.Contract.Operations;
            foreach (var operation in operations)
            {
                operation.Behaviors.Find<DataContractSerializerOperationBehavior>().MaxItemsInObjectGraph = int.MaxValue;
            }
            return factory.CreateChannel();
        }

        public static T CreateClient<T>(string serviceEndpoint)
        {
            Uri uri = new Uri(serviceEndpoint);
            BindingType bindingType = GetBindingType(uri.Scheme);
            return CreateClient<T>(serviceEndpoint, bindingType);
        }

        private static BindingType GetBindingType(string scheme)
        {
            switch (scheme)
            { 
                case "http":
                    return BindingType.WsHttp;
                case "https":
                    return BindingType.Https;
                case "net.tcp":
                    return BindingType.NetTcp;
                default:
                    throw new NotSupportedException(string.Format("The scheme {0} is not supported", scheme));
            }
        }

        private static Binding GetClientBinding(BindingType bindingType)
        {
            switch (bindingType)
            { 
                case BindingType.WsHttp:
                    WSHttpBinding wsHttpBinding = new WSHttpBinding(SecurityMode.None);
                    wsHttpBinding.MaxReceivedMessageSize = int.MaxValue;
                    return wsHttpBinding;
                case BindingType.Https:
                    WSHttpBinding wsHttpBinding2 = new WSHttpBinding(SecurityMode.Transport);
                    wsHttpBinding2.MaxReceivedMessageSize = int.MaxValue;
                    return wsHttpBinding2;
                case BindingType.NetTcp:
                    NetTcpBinding netTcpBinding = new NetTcpBinding(SecurityMode.None);
                    netTcpBinding.MaxReceivedMessageSize = int.MaxValue;
                    return netTcpBinding;
                default:
                    throw new NotSupportedException(string.Format("The bindingType {0} is not supported", bindingType));
            }
        }
    }
}
