using System;
using EnCor.Configuration;
using System.Configuration;
using System.Xml;
using EnCor.ObjectBuilder;

namespace EnCor.Security
{
    public class AuthorizationProviderConfigCollection : PolymorphicConfigurationElementCollection<AuthorizationProviderConfig>
    {
        private const string StrProvider = "provider";

        public AuthorizationProviderConfigCollection()
        {
            AddElementName = StrProvider;
        }
    }
}
