using System.Configuration;

namespace EnCor.Security
{
    public class AuthorizationConfig : ConfigurationElement
    {
        private const string StrProviders = "providers";

        [ConfigurationProperty(StrProviders)]
        public AuthorizationProviderConfigCollection Providers
        {
            get
            {
                return (AuthorizationProviderConfigCollection)this[StrProviders];
            }
        }
    }
}
