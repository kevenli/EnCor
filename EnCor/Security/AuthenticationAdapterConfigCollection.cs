using System;
using System.Configuration;
using EnCor.Configuration;
using EnCor.ObjectBuilder;
using System.Xml;

namespace EnCor.Security
{
    public class AuthenticationAdapterConfigCollection : PolymorphicConfigurationElementCollection<AuthenticationAdapterConfig>
    {
        private const string StrAdapter = "adapter";
        public AuthenticationAdapterConfigCollection()
        {
            AddElementName = StrAdapter;
        }
    }
}
