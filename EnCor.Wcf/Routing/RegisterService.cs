using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Channels;
using EnCor.Wcf.NodeHosting;
using System.Xml;
using log4net.Core;
using EnCor.Logging;
using EnCor.Wcf.Routing.Algorithms;

namespace EnCor.Wcf.Routing
{
    public class RegisterService : IRegistrationService
    {
        /// <summary>
        /// 按ContractNamespace分组注册的Service列表
        /// </summary>
        private static IDictionary<string, List<RegistrationInfo>> GroupRegisterServiceList = new Dictionary<string, List<RegistrationInfo>>();
        private static IDictionary<UniqueId, List<Uri>> CalledRegisterServiceList = new Dictionary<UniqueId, List<Uri>>();
        private static IDictionary<string, ServiceNodeInfo> ServiceNodeList = new Dictionary<string, ServiceNodeInfo>();
        private static object obj = new object();

        private DynamicNodesProvider _nodesProvider;

        public RegisterService(DynamicNodesProvider nodesProvider)
        {
            _nodesProvider = nodesProvider;
        }

        /// <summary>
        /// 已注册的Service列表
        /// </summary>
        private static IDictionary<int, RegistrationInfo> RegistrationList = new Dictionary<int, RegistrationInfo>();
        private IDictionary<string, int> RoundRobinCount = new Dictionary<string, int>();

        public void Register(RegistrationInfo regInfo, int proportion)
        {
            _nodesProvider.RegisterNode(new NodeInfo(){Action = regInfo.ContractNamespace, Address = regInfo.Address});
            //lock (RegistrationList)
            //{
            //    if (!RegistrationList.ContainsKey(regInfo.GetHashCode()))
            //    {
            //        RegistrationList.Add(regInfo.GetHashCode(), regInfo);

            //        lock (GroupRegisterServiceList)
            //        {
            //            List<RegistrationInfo> list = null;
            //            if (GroupRegisterServiceList.ContainsKey(regInfo.ContractNamespace))
            //            {
            //                list = GroupRegisterServiceList[regInfo.ContractNamespace];
            //                list.Add(regInfo);
            //            }
            //            else
            //            {
            //                list = new List<RegistrationInfo>();
            //                list.Add(regInfo);
            //                GroupRegisterServiceList.Add(regInfo.ContractNamespace, list);
            //            }
            //        }

            //        lock (ServiceNodeList)
            //        {
            //            if (!ServiceNodeList.ContainsKey(regInfo.BaseAddress))
            //            {
            //                ServiceNodeInfo node = new ServiceNodeInfo();
            //                node.BaseAddress = regInfo.BaseAddress;
            //                node.LastCalledTime = DateTime.Now;
            //                node.Proportion = proportion;

            //                ServiceNodeList.Add(regInfo.BaseAddress, node);
            //            }
            //        }
            //    }
            //    Console.WriteLine("Register : {0} {1} {2}", regInfo.ContractName, regInfo.Address, regInfo.BindingName);
            //    Runtime.Logging.Info("RegisterService.Register(),  Address=" + regInfo.Address);  
            //}
            Runtime.Logging.Info(string.Format("RegisterService.Register(),  Address={0}, ContractNamespace={1}, ContractName={2}", regInfo.Address, regInfo.ContractNamespace, regInfo.ContractName));  
        }

        public void Unregister(RegistrationInfo regInfo)
        {
            _nodesProvider.UngisterNode(new NodeInfo() { Action = regInfo.ContractNamespace, Address = regInfo.Address });
            Runtime.Logging.Info(string.Format("RegisterService.Unregister(),  Address={0}, ContractNamespace={1}, ContractName={2}", regInfo.Address, regInfo.ContractNamespace, regInfo.ContractName));  
            //lock (RegistrationList)
            //{
            //    if (RegistrationList.ContainsKey(regInfo.GetHashCode()))
            //    {
            //        RegistrationList.Remove(regInfo.GetHashCode());
            //    }
            //    Console.WriteLine("Unregister : {0} {1} {2}", regInfo.ContractName, regInfo.Address, regInfo.BindingName);
            //    Runtime.Logging.Info("RegisterService.Unregister(),  Address=" + regInfo.Address);  
            //}
        }

        public int HeartCheck(string baseAddress)
        {
            int count = 0;
            foreach (RegistrationInfo info in RegistrationList.Values)
            {
                if (info.BaseAddress == baseAddress)
                    count += 1;
            }

            foreach (ServiceNodeInfo node in ServiceNodeList.Values)
            {
                if (node.BaseAddress == baseAddress)
                    node.LastCalledTime = DateTime.Now;
            }

            return count;
        }

        private static int GetProportion(string baseAddress)
        {
            int proportion = 0;
            if (ServiceNodeList.ContainsKey(baseAddress))
            {
                try
                {
                    proportion = ServiceNodeList[baseAddress].Proportion;
                }
                catch { }
            }

            if (proportion < 1)
            {
                proportion = 100;
            }

            return proportion;
        }

        public static void StartServiceInvoke(int invokeServiceHasCode)
        {
            if (RegistrationList.ContainsKey(invokeServiceHasCode))
            {
                lock (obj)
                {
                    RegistrationList[invokeServiceHasCode].InvokeCount += 1;
                }
                //Console.WriteLine(string.Format("StartServiceInvoke,HasCode=[{0}] InvokeCount=[{1}]", invokeServiceHasCode, RegistrationList[invokeServiceHasCode].InvokeCount));
            }
        }

        public static void EndServiceInvoke(int invokeServiceHasCode)
        {
            if (RegistrationList.ContainsKey(invokeServiceHasCode))
            {
                lock (obj)
                {
                    RegistrationList[invokeServiceHasCode].InvokeCount -= 1;
                }
                //Console.WriteLine(string.Format("EndServiceInvoke,HasCode=[{0}] InvokeCount=[{1}]", invokeServiceHasCode, RegistrationList[invokeServiceHasCode].InvokeCount));
            }
        }

        public static void KeepCalledService(UniqueId messageId, Uri uri)
        {
            try
            {
                lock (CalledRegisterServiceList)
                {
                    if (CalledRegisterServiceList.ContainsKey(messageId))
                    {
                        CalledRegisterServiceList[messageId].Add(uri);
                    }
                    else
                    {
                        List<Uri> list = new List<Uri>();
                        list.Add(uri);
                        CalledRegisterServiceList.Add(messageId, list);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("KeepCalledService Exception: {0}", ex.Message));
                Runtime.Logging.Error("KeepCalledService Exception", ex);
            }
        }

        public static void RemoveCalledService(UniqueId messageId)
        {
            try
            {
                if (CalledRegisterServiceList.ContainsKey(messageId))
                    CalledRegisterServiceList.Remove(messageId);
            }
            catch(Exception ex)
            {
                Console.WriteLine(string.Format("RemoveCalledService Exception: {0}", ex.Message));
                Runtime.Logging.Error("RemoveCalledService Exception", ex);
            }
        }

        public static void ClearTimeOutService()
        {
            try
            {
                IDictionary<string, ServiceNodeInfo> removeNodeList = new Dictionary<string, ServiceNodeInfo>();
                foreach (string nodeKey in ServiceNodeList.Keys)
                {
                    ServiceNodeInfo node = ServiceNodeList[nodeKey];
                    TimeSpan ts = DateTime.Now - node.LastCalledTime;
                    TimeSpan timeout = RouterHost.GetRouterTimeOut("heartTimeout");
                    if (ts > timeout)
                    {
                        removeNodeList.Add(nodeKey, node);

                        IDictionary<int, RegistrationInfo> dic = new Dictionary<int, RegistrationInfo>();
                        foreach (int key in RegistrationList.Keys)
                        {
                            RegistrationInfo info = RegistrationList[key];

                            if (node.BaseAddress == info.BaseAddress)
                                dic.Add(key, info);
                        }

                        foreach (int key in dic.Keys)
                        {
                            lock (RegistrationList)
                            {
                                RegistrationList.Remove(key);
                            }
                        }


                        foreach (List<RegistrationInfo> list in GroupRegisterServiceList.Values)
                        {
                            List<RegistrationInfo> tempList = new List<RegistrationInfo>();
                            foreach (RegistrationInfo info in list)
                            {
                                if (info.BaseAddress == node.BaseAddress)
                                    tempList.Add(info);
                            }

                            foreach (RegistrationInfo info in tempList)
                            {
                                lock (GroupRegisterServiceList)
                                {
                                    list.Remove(info);
                                }
                            }
                        }

                        Console.WriteLine(string.Format("ClearOutTimeService At The Node Of - {0}", node.BaseAddress));
                        Runtime.Logging.Info("RegisterService.ClearOutTimeService(),  Node Address=" + node.BaseAddress);
                    }

                }

                foreach (string nodeKey in removeNodeList.Keys)
                {
                    lock (ServiceNodeList)
                    {
                        ServiceNodeList.Remove(nodeKey);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("ClearTimeOutService Exception: {0}", ex.Message));
                Runtime.Logging.Error("ClearTimeOutService Exception", ex);
            }
        }
    }
}
