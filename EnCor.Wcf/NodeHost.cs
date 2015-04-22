using System;
using System.Collections.Generic;
using System.Text;
using EnCor.Hosting;
using EnCor.ServiceLocator;
using EnCor.Wcf.NodeHosting;
using EnCor.ObjectBuilder;
using EnCor.Wcf.Hosting;
using System.ServiceModel;
using System.ServiceModel.Channels;
using EnCor.Wcf.Routing;
using System.ServiceModel.Dispatcher;
using EnCor.Logging;
using System.IO;
using System.ServiceModel.Configuration;
using System.Configuration;
using System.ServiceModel.Description;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using System.Net;
using System.Reflection;

namespace EnCor.Wcf
{
    [AssembleConfig(typeof(NodeHostConfig))]
    public class NodeHost : Module
    {
        private List<ServiceHost> _Hosts = new List<ServiceHost>();
        private static NodeHostConfig _Config;
        private Thread monitorThread;
        private static Dictionary<string, int> _NodeSetting = new Dictionary<string, int>();
        private bool _NeedRegister = false;

        public NodeHost(NodeHostConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

            _Config = config;

            _NeedRegister = config.RegisterAddresses.Count > 0;
            //monitorThread = new Thread(new ThreadStart(MonitorMain));
        }

        System.Reflection.Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            foreach (var dir in Directory.GetDirectories(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "shadow")))
            {// find assembly in shadow folders: "shadow\MODULE\bin"
                string folderPath = Path.Combine(dir, "bin");
                string assemblyPath = Path.Combine(folderPath, new AssemblyName(args.Name).Name + ".dll");
                if (File.Exists(assemblyPath))
                {
                    Assembly assembly = Assembly.LoadFrom(assemblyPath);
                    return assembly;
                }
            }
            return null;
        }

        private void CreateHosts()
        {
            foreach (ServiceConfig serviceConfig in _Config.ServiceNodes)
            {
                ServiceHost host = null;
                if (!string.IsNullOrEmpty(serviceConfig.ServiceRef))
                {
                    host = BuildHostForServiceRef(serviceConfig);
                }
                else
                {
                    host = BuildHost(serviceConfig);
                }

                ServiceThrottlingBehavior throttleBehavior = host.Description.Behaviors.Find<ServiceThrottlingBehavior>();
                if (throttleBehavior == null)
                {
                    throttleBehavior = new ServiceThrottlingBehavior
                    {
                        MaxConcurrentCalls = 500,
                        MaxConcurrentInstances = 500,
                        MaxConcurrentSessions = 180
                    };
                    host.Description.Behaviors.Add(throttleBehavior);
                }

                AddHost(host);
            }
        }

        private ServiceHost BuildHost(ServiceConfig serviceConfig)
        {
            if (string.IsNullOrEmpty(serviceConfig.ServiceRef) && serviceConfig.Type == null)
            {
                Runtime.Logging.Error("NodeHost.BuildHost(),  serviceConfig.Type is null");
                throw new Exception(string.Format("Cannot find service type {0}", serviceConfig.TypeName));
            }

            List<Uri> baseUriList = new List<Uri>();
            foreach (Uri baseUri in _Config.BaseAddressesUri)
            {
                Uri newInstanceUri;
                if (!string.IsNullOrEmpty(serviceConfig.Address))
                {
                    newInstanceUri = new Uri(baseUri, serviceConfig.Address);
                }
                else
                {
                    newInstanceUri = new Uri(baseUri, serviceConfig.Name);
                }
                baseUriList.Add(newInstanceUri);
            }

            ServiceHost host = null;    // the built host to return
            Type serviceType = null;    // service actual type
            Type[] types = null;        // service contracts
            serviceType = serviceConfig.Type;
            host = new ServiceHost(serviceConfig.Type, baseUriList.ToArray());
            types = GetServiceContracts(serviceType, serviceConfig);

            // add error logger.
            host.Description.Behaviors.Add(new WcfErrorHandler());

            
            foreach (Type contractType in types)
            {
                foreach (Uri baseAddress in baseUriList)
                {
                    if (!HostContainsSchemeEndpoint(host, baseAddress.Scheme))
                    {
                        //Uri routerUri = GetRouterUri(baseAddress);
                        Uri endpointUri = GenerateEndpointUniqueAddress(baseAddress, serviceType.Name);
                        Binding binding = GetEndpointBinding(baseAddress.Scheme, false);
                        ServiceEndpoint serviceEndpoint = host.AddServiceEndpoint(contractType, binding, endpointUri, endpointUri);
                        serviceEndpoint.Behaviors.Add(new MatchAllAddressBehavior());

                        foreach (OperationDescription operation in serviceEndpoint.Contract.Operations)
                        {
                            operation.Behaviors.Find<DataContractSerializerOperationBehavior>().MaxItemsInObjectGraph = int.MaxValue;
                        }
                    }
                }
            }
            return host;
        }

        private ServiceHost BuildHostForServiceRef(ServiceConfig serviceConfig)
        {
            if (string.IsNullOrEmpty(serviceConfig.ServiceRef) && serviceConfig.Type == null)
            {
                Runtime.Logging.Error("NodeHost.BuildHost(),  serviceConfig.Type is null");
                throw new Exception(string.Format("Cannot find service type {0}", serviceConfig.TypeName));
            }

            List<Uri> baseUriList = new List<Uri>();
            foreach (Uri baseUri in _Config.BaseAddressesUri)
            {
                Uri newInstanceUri;
                if (!string.IsNullOrEmpty(serviceConfig.Address))
                {
                    newInstanceUri = new Uri(baseUri, serviceConfig.Address);
                }
                else
                {
                    newInstanceUri = new Uri(baseUri, serviceConfig.Name);
                }
                baseUriList.Add(newInstanceUri);
            }

            ServiceHost host = null;
            Type serviceType = null;
            Type[] types = null;
            object serviceInstance = Runtime.GetService(serviceConfig.ServiceRef);
            if (serviceInstance == null)
            {
                throw new Exception(string.Format("Cannot retrieve service named :", serviceConfig.ServiceRef));
            }
            host = new ServiceHost(serviceInstance);

            // set ContextMode to single
            var behaviour = host.Description.Behaviors.Find<ServiceBehaviorAttribute>();
            behaviour.InstanceContextMode = InstanceContextMode.Single;

            types = new Type[] { Runtime.GetServiceInterface(serviceInstance) };

            // add error logger.
            host.Description.Behaviors.Add(new WcfErrorHandler());


            foreach (Type contractType in types)
            {
                object[] attrs = contractType.GetCustomAttributes(typeof(ServiceContractAttribute), false);
                bool isDuplex = false;

                if (attrs.Length > 0)
                {
                    isDuplex = (attrs[0] as ServiceContractAttribute).CallbackContract != null;
                }

                foreach (Uri baseAddress in baseUriList)
                {
                    if (!HostContainsSchemeEndpoint(host, baseAddress.Scheme))
                    {
                        //Uri routerUri = GetRouterUri(baseAddress);
                        Uri endpointUri = baseAddress;
                        Binding binding = GetEndpointBinding(baseAddress.Scheme, isDuplex);
                        ServiceEndpoint serviceEndpoint = host.AddServiceEndpoint(contractType, binding, endpointUri, endpointUri);
                        serviceEndpoint.Behaviors.Add(new MatchAllAddressBehavior());

                        foreach (OperationDescription operation in serviceEndpoint.Contract.Operations)
                        {
                            operation.Behaviors.Find<DataContractSerializerOperationBehavior>().MaxItemsInObjectGraph = int.MaxValue;
                        }
                    }
                }
            }
            return host;
        }

        private static bool HostContainsSchemeEndpoint(ServiceHost host, string scheme)
        {
            foreach (var endpoint in host.Description.Endpoints)
            {
                if (endpoint.Address.Uri.Scheme == scheme)
                {
                    return true;
                }
            }
            return false;
        }

        private Binding GetEndpointBinding(string scheme, bool duplex)
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
                if (duplex)
                {
                    WSDualHttpBinding binding = new WSDualHttpBinding(WSDualHttpSecurityMode.None);
                    binding.MaxBufferPoolSize = int.MaxValue;
                    binding.MaxReceivedMessageSize = int.MaxValue;
                    return binding;
                }
                // b = new WSHttpBinding(SecurityMode.None, false);
                WSHttpBinding wsBind = new WSHttpBinding(SecurityMode.None, false);
                wsBind.MaxBufferPoolSize = int.MaxValue;
                wsBind.MaxReceivedMessageSize = int.MaxValue;

                return wsBind;
            }
            else if (scheme == "net.tcp")
            {
                NetTcpBinding tcpBinding = new NetTcpBinding(SecurityMode.None);
                tcpBinding.Security.Message.ClientCredentialType = MessageCredentialType.None;
                return tcpBinding;
            }
            else if (scheme == "net.pipe")
            {
                b = new CustomBinding(new BinaryMessageEncodingBindingElement(), new NamedPipeTransportBindingElement { ManualAddressing = true });
            }
            else if (scheme == "https")
            {
                b = new CustomBinding(new TextMessageEncodingBindingElement(), new HttpsTransportBindingElement { ManualAddressing = true });
            }

            return b;
        }

        private Uri GenerateEndpointUniqueAddress(Uri baseAddress, string serviceName)
        {
            return baseAddress;
        }

        private Type[] GetServiceContracts(Type serviceType, ServiceConfig config)
        {
            if (!string.IsNullOrEmpty(config.Contract))
            {
                Type contractType = Type.GetType(config.Contract);
                if (contractType == null)
                {
                    Runtime.Logging.Error("NodeHost.GetServiceContracts(),  contractType is null");
                    throw new Exception(string.Format("Cannot find contract type: {0}", config.Contract));
                }
                return new Type[] { contractType };
            }

            List<Type> contractTypes = new List<Type>();

            if (serviceType.GetCustomAttributes(typeof(ServiceContractAttribute), false).Length > 0)
            {
                contractTypes.Add(serviceType);
            }
            foreach (Type interfaceType in serviceType.GetInterfaces())
            {
                if (interfaceType.GetCustomAttributes(typeof(ServiceContractAttribute), false).Length > 0)
                {
                    contractTypes.Add(interfaceType);
                }
            }
            return contractTypes.ToArray();
        }

        public override void Start()
        {
            Runtime.Logging.Info("Host starting.");
            CreateHosts();

            foreach (ServiceHost host in _Hosts)
            {
                //Runtime.Logging.Info("NodeHost.Start(),  Host Open :" + host.Description.Name );  
                host.Open();
                DisplayHostInfo(host);
            }

            RegisterWithRouter();

            if (monitorThread != null && _Config.RegisterAddresses.Count > 0)
                monitorThread.Start();
        }

        public override void Stop()
        {
            foreach (ServiceHost host in _Hosts)
            {
                //Runtime.Logging.Info("NodeHost.Start(),  Host Stop :" + host.Description.Name);  

                if (host.State == CommunicationState.Faulted)
                    host.Abort();
                else
                    host.Close();
                UnregisterWithRouter(host);
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
            _Hosts.Clear();
        }

        protected void AddHost(ServiceHost host)
        {
            _Hosts.Add(host);
        }

        private void RegisterWithRouter()
        {
            if (!_NeedRegister)
            {
                return;
            }
            try
            {
                Uri baseAddressUri = new Uri(_Config.RegisterAddresses[0].BaseAddress);
                Binding routerBinding = GetEndpointBinding(baseAddressUri.Scheme, false);
                int proportion = GetNodeSetting("proportion");

                using (ChannelFactory<IRegistrationService> factory = new ChannelFactory<IRegistrationService>(routerBinding, new EndpointAddress(_Config.RegisterAddresses[0].BaseAddress)))
                {
                    IRegistrationService proxy = factory.CreateChannel();

                    using (proxy as IDisposable)
                    {
                        foreach (ServiceHost host in _Hosts)
                        {
                            try
                            {
                                if (host.State != CommunicationState.Opened)
                                    host.Open();

                                string baseAddress = "";
                                if (_Config.BaseAddressesUri.Length > 0)
                                    baseAddress = _Config.BaseAddressesUri[0].OriginalString;

                                foreach (ChannelDispatcher dispatcher in host.ChannelDispatchers)
                                {

                                    Console.WriteLine("Registering endpoints:");
                                    Console.WriteLine("\t{0}", dispatcher.Listener.Uri.AbsoluteUri);
                                    foreach (EndpointDispatcher endpointDispatcher in dispatcher.Endpoints)
                                    {
                                        Console.WriteLine("\t{0}, {1}", endpointDispatcher.ContractName, endpointDispatcher.ContractNamespace);


                                        proxy.Register(new RegistrationInfo
                                        {
                                            Address = dispatcher.Listener.Uri.AbsoluteUri,
                                            BindingName = dispatcher.BindingName,
                                            ContractName = endpointDispatcher.ContractName,
                                            ContractNamespace = string.Format("{0}/{1}", endpointDispatcher.ContractNamespace.TrimEnd('/'), endpointDispatcher.ContractName),
                                            
                                            BaseAddress = baseAddress
                                        }, proportion);
                                    }
                                    Console.WriteLine();
                                }

                                //Console.WriteLine("Dispatch listeners: {0}", host.ChannelDispatchers.Count);
                                //DisplayHostInfo(host);
                            }
                            catch (Exception ex)
                            {
                                Runtime.Logging.Error("NodeHost.RegisterWithRouter(), foreach (ServiceHost host in _Hosts) Exception ", ex);
                                Console.WriteLine("NodeHost.RegisterWithRouter(), foreach (ServiceHost host in _Hosts) Exception " + ex.Message);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Runtime.Logging.Error("NodeHost.RegisterWithRouter(),using (ChannelFactory<IRegistrationService> Exception ", ex);
                Console.WriteLine("NodeHost.RegisterWithRouter(), using (ChannelFactory<IRegistrationService> Exception:" + ex.Message);
            }
        }

        private void DisplayHostInfo(ServiceHost host)
        {
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

        private void UnregisterWithRouter(ServiceHost host)
        {
            if (!_NeedRegister)
            {
                return;
            }
            Uri baseAddressUri = new Uri(_Config.RegisterAddresses[0].BaseAddress);
            Binding routerBinding = GetEndpointBinding(baseAddressUri.Scheme, false);

            try
            {
                using (ChannelFactory<IRegistrationService> factory = new ChannelFactory<IRegistrationService>(routerBinding, new EndpointAddress(_Config.RegisterAddresses[0].BaseAddress)))
                {
                    IRegistrationService proxy = factory.CreateChannel();

                    using (proxy as IDisposable)
                    {
                        string baseAddress = "";
                        if (_Config.BaseAddressesUri.Length > 0)
                            baseAddress = _Config.BaseAddressesUri[0].OriginalString;

                        foreach (ChannelDispatcher dispatcher in host.ChannelDispatchers)
                        {

                            Console.WriteLine("Unregistering endpoints:");
                            Console.WriteLine("\t{0}", dispatcher.Listener.Uri.AbsoluteUri);
                            foreach (EndpointDispatcher endpointDispatcher in dispatcher.Endpoints)
                            {
                                Console.WriteLine("\t{0}, {1}", endpointDispatcher.ContractName, endpointDispatcher.ContractNamespace);


                                proxy.Unregister(new RegistrationInfo
                                {
                                    Address = dispatcher.Listener.Uri.AbsoluteUri,
                                    BindingName = dispatcher.BindingName,
                                    ContractName = endpointDispatcher.ContractName,
                                    ContractNamespace = string.Format("{0}/{1}", endpointDispatcher.ContractNamespace, endpointDispatcher.ContractName),
                                    BaseAddress = baseAddress
                                });
                            }
                            Console.WriteLine();
                        }
                    }
                }
            }

            catch (CommunicationObjectFaultedException ex)
            {
                Runtime.Logging.Error("Error when unregister service, CommunicationObjectFaultedException ", ex);
            }
            catch (EndpointNotFoundException ex)
            {
                Runtime.Logging.Error("Error when unregister service : EndpointNotFoundException ", ex);
            }
            catch (Exception ex)
            {
                Runtime.Logging.Error("NodeHost.UnregisterWithRouter(), Exception ", ex);
                Console.WriteLine("Unregistering Exception:" + ex.Message);
            }
        }

        public static int GetNodeSetting(string attribute)
        {
            if (_NodeSetting.ContainsKey(attribute))
            {
                return _NodeSetting[attribute];
            }
            else
            {
                string value = _Config.NodeSetting.XmlElement.Attributes[attribute].Value;
                int minSecond = Convert.ToInt32(value);
                _NodeSetting.Add(attribute, minSecond);
                return minSecond;
            }
        }


        public void MonitorMain()
        {
            do
            {
                int minSecond = GetNodeSetting("monitorSleep");

                try
                {
                    int hostServiceCount = _Hosts.Count;

                    string registerAddress = _Config.RegisterAddresses[0].BaseAddress;
                    string baseAddress = _Config.BaseAddressesUri[0].OriginalString;

                    string scheme = _Config.RegisterAddresses[0].BaseAddress.Split(':')[0];
                    Binding routerBinding = GetEndpointBinding(scheme, false);

                    using (ChannelFactory<IRegistrationService> factory = new ChannelFactory<IRegistrationService>(routerBinding, new EndpointAddress(registerAddress)))
                    {
                        IRegistrationService proxy = factory.CreateChannel();

                        using (proxy as IDisposable)
                        {

                            while (true)
                            {
                                Thread.Sleep(minSecond);
                                int count = proxy.HeartCheck(baseAddress);
                                if (hostServiceCount > count)
                                    RegisterWithRouter();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(string.Format("MonitorMain Exception: {0}", ex.Message));
                    Thread.Sleep(minSecond);

                    Console.WriteLine("Try Connection RouterService.");
                }
            }
            while (true);
        }
    }
}
