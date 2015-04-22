using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel;

namespace EnCor.Wcf.Routing
{
    public class ConfigurationUtility
    {
        public static Binding GetRouterBinding(string scheme)
        {
            Binding b = null;
            TextMessageEncodingBindingElement textBindElement = new TextMessageEncodingBindingElement();
            textBindElement.ReaderQuotas.MaxArrayLength = int.MaxValue;
            textBindElement.ReaderQuotas.MaxBytesPerRead = int.MaxValue;
            textBindElement.ReaderQuotas.MaxDepth = int.MaxValue;
            textBindElement.ReaderQuotas.MaxNameTableCharCount = int.MaxValue;
            textBindElement.ReaderQuotas.MaxStringContentLength = int.MaxValue;
            textBindElement.MaxReadPoolSize = int.MaxValue;
            textBindElement.MaxWritePoolSize = int.MaxValue;

            if (scheme == "http")
            {
                b = new CustomBinding(textBindElement, new HttpTransportBindingElement
                {
                    ManualAddressing = true,
                    MaxBufferPoolSize = int.MaxValue,
                    MaxReceivedMessageSize = int.MaxValue
                });
            }
            else if (scheme == "net.tcp")
            {
                NetTcpBinding tcpBinding = new NetTcpBinding(SecurityMode.None);
                tcpBinding.Security.Message.ClientCredentialType = MessageCredentialType.None;
                tcpBinding.MaxReceivedMessageSize = int.MaxValue;

                tcpBinding.ReceiveTimeout = RouterHost.GetRouterTimeOut("receiveTimeout");
                tcpBinding.OpenTimeout = RouterHost.GetRouterTimeOut("openTimeout");
                tcpBinding.SendTimeout = RouterHost.GetRouterTimeOut("sendTimeout");
                tcpBinding.CloseTimeout = RouterHost.GetRouterTimeOut("closeTimeout");
                return tcpBinding;
            }
            else if (scheme == "net.pipe")
            {
                b = new CustomBinding(textBindElement, new NamedPipeTransportBindingElement
                {
                    ManualAddressing = true,
                    MaxBufferPoolSize = int.MaxValue,
                    MaxReceivedMessageSize = int.MaxValue
                });
            }
            else if (scheme == "https")
            {
                b = new CustomBinding(textBindElement, new HttpsTransportBindingElement
                {
                    ManualAddressing = true,
                    MaxBufferPoolSize = int.MaxValue,
                    MaxReceivedMessageSize = int.MaxValue
                });
            }
            b.ReceiveTimeout = RouterHost.GetRouterTimeOut("receiveTimeout");
            b.OpenTimeout = RouterHost.GetRouterTimeOut("openTimeout");
            b.SendTimeout = RouterHost.GetRouterTimeOut("sendTimeout");
            b.CloseTimeout = RouterHost.GetRouterTimeOut("closeTimeout");

            return b;
        }
    }
}
