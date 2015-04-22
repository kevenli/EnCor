using System;
using System.Collections.Generic;
using System.Text;
using EnCor.Configuration;
using EnCor.ModuleLoader;
using EnCor.ObjectBuilder;
using EnCor.Hosting;
using System.Configuration;
using System.ServiceModel.Configuration;
using EnCor.Wcf.NodeHosting;

namespace EnCor.Wcf.Routing
{
    public class RouterHostConfig : ModuleConfig, IModuleAssembler
    {
        [ConfigurationProperty("routerAddresses")]
        public System.ServiceModel.Configuration.BaseAddressElementCollection RouterAddresses
        {
            get
            {
                return (BaseAddressElementCollection)this["routerAddresses"];
            }
        }

        [ConfigurationProperty("registerAddresses")]
        public System.ServiceModel.Configuration.BaseAddressElementCollection RegisterAddresses
        {
            get
            {
                return (BaseAddressElementCollection)this["registerAddresses"];
            }
        }

        [ConfigurationProperty("cerficate")]
        public System.ServiceModel.Configuration.CertificateReferenceElement Cerficate
        {
            get
            {
                return (CertificateReferenceElement)this["cerficate"];
            }
        }

        [ConfigurationProperty("routerTimeOut")]
        public System.ServiceModel.Configuration.XmlElementElement RouterTimeOut
        {
            get
            {
                return (XmlElementElement)this["routerTimeOut"];
            }
        }

        [ConfigurationProperty("routerSetting")]
        public System.ServiceModel.Configuration.XmlElementElement RouterSetting
        {
            get
            {
                return (XmlElementElement)this["routerSetting"];
            }
        }

        [ConfigurationProperty("nodes")]
        public NodeConfigCollection Nodes
        {
            get
            {
                return (NodeConfigCollection)this["nodes"];
            }
        }

        [ConfigurationProperty("autoDelist")]
        public AutoDelistConfig AutoDelist
        {
            get { return (AutoDelistConfig) this["autoDelist"]; }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue">00:02:03</param>
        /// <returns>TimeSpan</returns>
        public static TimeSpan ConvertTimeSpan(string strValue)
        {
            string[] array = strValue.Split(':');
            int hour = 0;
            int min = 0;
            int sec = 0;
            try
            {
                hour = Convert.ToInt32(array[0]);
                min = Convert.ToInt32(array[1]);
                sec = Convert.ToInt32(array[2]);
            }
            catch { }

            TimeSpan ts = new TimeSpan(hour, min, sec);
            return ts;
        }

        #region IAssembler<FacadeHost,FacadeHostConfiguration> Members

        public IEnCorModule Assemble(IBuilderContext context, IModuleConfig objectConfiguration)
        {
            RouterHostConfig config = objectConfiguration as RouterHostConfig;
            if (config == null)
            {
                throw new NotSupportedException(string.Format("The config is not RouterHostConfig : {0}", objectConfiguration));
            }
            RouterHost host = new RouterHost(config);
            return host;

        }

        #endregion
    }
}
