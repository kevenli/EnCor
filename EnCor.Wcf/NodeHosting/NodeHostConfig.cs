using System;
using System.Collections.Generic;
using System.Text;
using EnCor.Configuration;
using EnCor.Hosting;
using EnCor.ModuleLoader;
using EnCor.ObjectBuilder;
using System.Configuration;
using System.ServiceModel.Configuration;
using EnCor.Wcf.Hosting;
using ServiceConfigCollection = EnCor.Wcf.Hosting.ServiceConfigCollection;

namespace EnCor.Wcf.NodeHosting
{
    public class NodeHostConfig : ModuleConfig, IModuleAssembler
    {
        [ConfigurationProperty("registerAddresses")]
        public System.ServiceModel.Configuration.BaseAddressElementCollection RegisterAddresses
        {
            get
            {
                return (BaseAddressElementCollection)this["registerAddresses"];
            }
        }

        [ConfigurationProperty("baseAddresses")]
        public System.ServiceModel.Configuration.BaseAddressElementCollection BaseAddresses
        {
            get
            {
                return (BaseAddressElementCollection)this["baseAddresses"];
            }
        }

        [ConfigurationProperty("nodeSetting")]
        public System.ServiceModel.Configuration.XmlElementElement NodeSetting
        {
            get
            {
                return (XmlElementElement)this["nodeSetting"];
            }
        }

        public Uri[] BaseAddressesUri
        {
            get
            {
                List<Uri> list = new List<Uri>(BaseAddresses.Count);
                foreach (BaseAddressElement element in BaseAddresses)
                {
                    list.Add(new Uri(element.BaseAddress));
                }
                return list.ToArray();
            }
        }

        [ConfigurationProperty("serviceNodes")]
        public ServiceConfigCollection ServiceNodes
        {
            get
            {
                return (ServiceConfigCollection)this["serviceNodes"];
            }
        }

        #region IAssembler<FacadeHost,FacadeHostConfiguration> Members

        public IEnCorModule Assemble(IBuilderContext context, IModuleConfig objectConfiguration)
        {
            NodeHostConfig config = objectConfiguration as NodeHostConfig;
            if (config == null)
            {
                throw new NotSupportedException(string.Format("The config is not RouterHostConfig : {0}", objectConfiguration));
            }
            NodeHost host = new NodeHost(config);
            return host;
        }

        #endregion
    }
}
