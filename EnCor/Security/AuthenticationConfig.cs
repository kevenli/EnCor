using System.Configuration;

namespace EnCor.Security
{
    public class AuthenticationConfig : ConfigurationElement
    {
        private const string StrProviders = "providers";
        private const string StrAdapters = "adapters";

        [ConfigurationProperty(StrProviders)]
        public AuthenticationProviderConfigCollection Providers
        {
            get
            {
                return (AuthenticationProviderConfigCollection)this[StrProviders];
            }
        }

        [ConfigurationProperty(StrAdapters)]
        public AuthenticationAdapterConfigCollection Adapters
        {
            get
            {
                return (AuthenticationAdapterConfigCollection)this[StrAdapters];
            }
        }
    }
}
