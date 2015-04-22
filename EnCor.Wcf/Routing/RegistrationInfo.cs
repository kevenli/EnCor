using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace EnCor.Wcf.Routing
{
    [DataContract(Namespace = "http://encor.codeplex.com/wcf/routing/")]
    public class RegistrationInfo
    {
        [DataMember(IsRequired = true, Order = 1)]
        public string Address { get; set; }

        [DataMember(IsRequired = true, Order = 2)]
        public string BindingName { get; set; }

        [DataMember(IsRequired = true, Order = 3)]
        public string ContractName { get; set; }

        [DataMember(IsRequired = true, Order = 4)]
        public string ContractNamespace { get; set; }

        [DataMember(IsRequired = true, Order = 5)]
        public string BaseAddress { get; set; }

        [DataMember(IsRequired = false, Order = 6)]
        public int InvokeCount { get; set; }

        public override int GetHashCode()
        {
            return this.Address.GetHashCode() + this.ContractName.GetHashCode() + this.ContractNamespace.GetHashCode() + this.BaseAddress.GetHashCode();
        }

        public static int SortByInvoke(RegistrationInfo a, RegistrationInfo b)
        {
            return a.InvokeCount.CompareTo(b.InvokeCount);
        }
    }
}
