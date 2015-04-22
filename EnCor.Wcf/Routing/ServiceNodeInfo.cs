using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace EnCor.Wcf.Routing
{
    [DataContract(Namespace = "EnCor.Wcf.Routing.ServiceNodeInfo")]
    public class ServiceNodeInfo
    {
        [DataMember(IsRequired = true, Order = 1)]
        public string BaseAddress { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        public DateTime LastCalledTime { get; set; }

        [DataMember(IsRequired = true, Order = 3)]
        public int Proportion { get; set; }

        public override int GetHashCode()
        {
            return this.BaseAddress.GetHashCode() + this.LastCalledTime.GetHashCode() + this.Proportion.GetHashCode();
        }
    }
}
