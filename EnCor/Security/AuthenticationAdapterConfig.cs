using System.Configuration;
using EnCor.Configuration;

namespace EnCor.Security
{
    public class AuthenticationAdapterConfig : NameTypeConfigElement
    {
        const string StrReadOnly = "readonly";
        [ConfigurationProperty(StrReadOnly, DefaultValue = false)]
        public bool ReadOnly
        {
            get
            {
                return (bool)this[StrReadOnly];
            }
        }
    }
}
