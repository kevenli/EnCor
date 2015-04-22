using System;
using System.Collections.Generic;
using System.Text;
using EnCor.Hosting;
using System.ServiceModel;
using EnCor.Wcf.Hosting;
using System.ServiceModel.Dispatcher;
using EnCor.ObjectBuilder;
using System.ServiceModel.Channels;
using EnCor.Wcf.NodeHosting;
using EnCor.Wcf.Routing;
using System.ServiceModel.Configuration;
using System.Threading;
using System.ServiceModel.Description;
using EnCor.Logging;
using EnCor.Wcf.Routing.Algorithms;

namespace EnCor.Wcf
{
    [AssembleConfig(typeof(RouterHostConfig))]
    public class RouterHost : Module
    {
        public static RouterHostConfig _Config;
        private ServiceHost _RouterHost;
        private ServiceHost _RegisterHost;
        private ServiceHost _DuplexRouterHost;
        private List<ServiceHost> _Hosts = new List<ServiceHost>(3);
        private Thread monitorThread;
        private static Dictionary<string, TimeSpan> _RouterTimeOut = new Dictionary<string, TimeSpan>();
        private static Dictionary<string, int> _RouterSetting = new Dictionary<string, int>();

        public RouterHost(RouterHostConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            _Config = config;
            monitorThread = new Thread(new ThreadStart(MonitorMain));
        }

        public override void Start()
        {
            List<Uri> routerAddresses = new List<Uri>();
            foreach (BaseAddressElement baseAddress in _Config.RouterAddresses)
            {
                routerAddresses.Add(new Uri(baseAddress.BaseAddress));
            }

            List<Uri> registerAddress = new List<Uri>();
            foreach (BaseAddressElement baseAddress in _Config.RegisterAddresses)
            {
                registerAddress.Add(new Uri(baseAddress.BaseAddress));
            }

            _RouterHost = new ServiceHost(typeof(RouterService));
            INodesProvider nodesProvider = BuildNodesProvider(_Config);
            INodePriorityAlgorithm algorithm = BuildAlgorithm(_Config);
            _RouterHost.Description.Behaviors.Add(new RouterServiceInstanceProvider(algorithm, nodesProvider));

            RegisterService registerService = new RegisterService((DynamicNodesProvider)nodesProvider);
            _RegisterHost = new ServiceHost(typeof(RegisterService));
            _RegisterHost.Description.Behaviors.Add(new SingleInstanceServiceBehavior(registerService));

            _DuplexRouterHost = new ServiceHost(typeof(DuplexRouterService));
            _DuplexRouterHost.Description.Behaviors.Add(new DuplexRouterServiceInstanceProvider(algorithm, nodesProvider));

            //ServiceThrottlingBehavior throttleBehavior = new ServiceThrottlingBehavior
            //{
            //    MaxConcurrentCalls = 500,
            //    MaxConcurrentInstances = 500,
            //     MaxConcurrentSessions = 180
            //};
            //_RouterHost.Description.Behaviors.Add(throttleBehavior);


            // routing service endpoints;
            foreach (Uri address in routerAddresses)
            {
                
                Binding binding = GetRouterServiceEndpointBinding(address.Scheme);
                switch (address.Scheme)
                { 
                    case "http":
                    case "https":
                    case "net.tcp":
                        _RouterHost.AddServiceEndpoint(typeof(IRouterService), binding, address);
                        //_RouterHost.Credentials.ServiceCertificate.SetCertificate(System.Security.Cryptography.X509Certificates.StoreLocation.LocalMachine, System.Security.Cryptography.X509Certificates.StoreName.My, System.Security.Cryptography.X509Certificates.X509FindType.FindBySubjectName, "lihao11");
                        if ( _Config.Cerficate != null && !string.IsNullOrEmpty(_Config.Cerficate.FindValue))
                        {
                            var cerficate = _Config.Cerficate;
                            _RouterHost.Credentials.ServiceCertificate.SetCertificate(cerficate.StoreLocation, cerficate.StoreName, cerficate.X509FindType, cerficate.FindValue);
                        }
                        if (!_Hosts.Contains(_RouterHost))
                        {
                            _Hosts.Add(_RouterHost);
                        }
                        break;
                    //case "net.tcp":
                    //    Binding b = new CustomBinding(new BinaryMessageEncodingBindingElement(), new TcpTransportBindingElement { ManualAddressing = true });
                    //    NetTcpBinding tcpBinding = new NetTcpBinding(SecurityMode.None);
                    //    tcpBinding.Security.Message.ClientCredentialType = MessageCredentialType.None;

                    //    tcpBinding.ReceiveTimeout = RouterHost.GetRouterTimeOut("receiveTimeout");
                    //    tcpBinding.OpenTimeout = RouterHost.GetRouterTimeOut("openTimeout");
                    //    tcpBinding.SendTimeout = RouterHost.GetRouterTimeOut("sendTimeout");
                    //    tcpBinding.CloseTimeout = RouterHost.GetRouterTimeOut("closeTimeout");

                    //    _DuplexRouterHost.AddServiceEndpoint(typeof(IDuplexRouterService), b, address);
                    //    if (!_Hosts.Contains(_DuplexRouterHost))
                    //    {
                    //        _Hosts.Add(_DuplexRouterHost);
                    //    }
                    //    break;
                    default:
                        throw new NotSupportedException(string.Format("Scheme {0} is not supported for routing", address.Scheme));
                }
                
            }

            foreach (Uri address in registerAddress)
            {
                Binding binding = GetRegisterServiceEndpointBinding(address.Scheme);
                _RegisterHost.AddServiceEndpoint(typeof(IRegistrationService), binding, address);
            }
            _Hosts.Add(_RegisterHost);

            //_Host.Faulted += new EventHandler(host_Faulted);

            try
            {
                OpenHosts();

                
                foreach (var host in _Hosts)
                {
                    Console.WriteLine("{0} started at :", host.Description.Name);
                    foreach (ChannelDispatcher dispatcher in host.ChannelDispatchers)
                    {

                        Console.WriteLine("\t{0}", dispatcher.Listener.Uri.AbsoluteUri);
                        Console.WriteLine("\t{0}", dispatcher.BindingName);
                        foreach (EndpointDispatcher endpointDispatcher in dispatcher.Endpoints)
                        {
                            Console.WriteLine("\t{0}, {1}", endpointDispatcher.ContractName, endpointDispatcher.ContractNamespace);
                        }
                        Console.WriteLine();
                    }
                }
            }

            catch (Exception ex)
            {
                CloseHosts();
                Console.WriteLine(ex.Message);
                throw;
            }

            if (monitorThread != null)
                monitorThread.Start();
        }

        private void CloseHosts()
        {
            foreach (var host in _Hosts)
            {
                Runtime.Logging.Info("RouterHost.CloseHosts(), Host Close : " + host.Description.Name);  

                if (host.State == CommunicationState.Faulted)
                    host.Abort();
                else
                    host.Close();
            }
        }

        private void OpenHosts()
        {
            foreach (var host in _Hosts)
            {
                Runtime.Logging.Info("RouterHost.OpenHosts(), Host Open : " + host.Description.Name);               
                host.Open();
            }
        }

        private void host_Faulted(object sender, EventArgs e)
        {
            Runtime.Logging.Error("RouterHost.host_Faulted(),  sender is " + sender.ToString());  
            throw new EnCorException("Host faulted.");
        }

        public override void Stop()
        {
            try
            {
                CloseHosts();
            }
            catch (Exception ex)
            {
                Runtime.Logging.Error("RouterHost.Stop(), Exception : " + ex.Message, ex);               
                Console.WriteLine("Error when closing : {0}", ex);
            }
            if (monitorThread != null)
            {
                try
                {
                    monitorThread.Abort();
                }
                catch (ThreadAbortException)
                {

                }
            }
        }

        private Binding GetRouterServiceEndpointBinding(string scheme)
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

            switch (scheme)
            {
                case "http":
                    b = new CustomBinding(textBindElement, new HttpTransportBindingElement
                    {
                        MaxBufferPoolSize = int.MaxValue,
                        MaxReceivedMessageSize = int.MaxValue
                    });
                    break;
                case "https":
                    b = new CustomBinding(textBindElement, new HttpsTransportBindingElement
                    {
                        MaxBufferPoolSize = int.MaxValue,
                        MaxReceivedMessageSize = int.MaxValue
                    });
                    break;
                case "net.tcp":
                    NetTcpBinding tcpBinding = new NetTcpBinding(SecurityMode.None);
                    tcpBinding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
                    //tcpBinding.Security.Message.ClientCredentialType = MessageCredentialType.None;
                    return tcpBinding;
                case "net.pipe":
                    b = new CustomBinding(textBindElement, new NamedPipeTransportBindingElement
                    {
                        MaxBufferPoolSize = int.MaxValue,
                        MaxReceivedMessageSize = int.MaxValue
                    });
                    break;
                default:
                    throw new NotSupportedException(string.Format("The scheme '{0}' is not supported as present.", scheme));
            }
            return b;
        }

        private Binding GetRegisterServiceEndpointBinding(string scheme)
        {
            Binding b = null;

            switch (scheme)
            {
                case "http":
                    b = new CustomBinding(new TextMessageEncodingBindingElement(), new HttpTransportBindingElement());
                    break;
                case "https":
                    b = new CustomBinding(new TextMessageEncodingBindingElement(), new HttpsTransportBindingElement());
                    break;
                case "net.tcp":
                    NetTcpBinding tcpBinding = new NetTcpBinding(SecurityMode.None);
                    tcpBinding.Security.Message.ClientCredentialType = MessageCredentialType.None;
                    return tcpBinding;
                case "net.pipe":
                    b = new CustomBinding(new BinaryMessageEncodingBindingElement(), new NamedPipeTransportBindingElement());
                    break;
                default:
                    throw new NotSupportedException(string.Format("The scheme '{0}' is not supported as present.", scheme));
            }
            return b;
        }


        public static TimeSpan GetRouterTimeOut(string attribute)
        {
            if (_RouterTimeOut.ContainsKey(attribute))
            {
                return _RouterTimeOut[attribute];
            }
            else
            {
                string timeout = RouterHost._Config.RouterTimeOut.XmlElement.Attributes[attribute].Value;
                TimeSpan ts = RouterHostConfig.ConvertTimeSpan(timeout);
                _RouterTimeOut.Add(attribute, ts);
                return ts;
            }
        }

        public static int GetRouterSetting(string attribute)
        {
            if (_RouterSetting.ContainsKey(attribute))
            {
                return _RouterSetting[attribute];
            }
            else
            {
                string value = RouterHost._Config.RouterSetting.XmlElement.Attributes[attribute].Value;
                int minSecond = Convert.ToInt32(value);
                _RouterSetting.Add(attribute, minSecond);
                return minSecond;
            }
        }


        public void MonitorMain()
        {
            int minSecond = GetRouterSetting("monitorSleep");
            while (true)
            {
                Thread.Sleep(minSecond);
                RegisterService.ClearTimeOutService();
            }
        }

        private INodePriorityAlgorithm BuildAlgorithm(RouterHostConfig config)
        {
            return new StandardNodePriorityAlgorithm();
        }

        private INodesProvider BuildNodesProvider(RouterHostConfig config)
        {
            List<NodeInfo> list = new List<NodeInfo>();
            config.Nodes.ForEach(x => list.Add(new NodeInfo { Name = x.Name, Action = x.ActionUri, Address = x.Address, Rate = x.Rate }));
            //INodesProvider nodesProvider = new StaticNodesProvider(list, config.AutoDelist);
            DynamicNodesProvider dynamicNodesProvider = new DynamicNodesProvider(list);
            return dynamicNodesProvider;
        }
    }
}
