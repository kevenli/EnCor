using System.Configuration;
using EnCor.ModuleLoader;
using EnCor.ObjectBuilder;

namespace EnCor.Security
{
    [Assembler(typeof(SecurityModuleAssembler))]
    public class SecurityModuleConfig : ModuleConfig
    {
        private const string StrAuthentication = "authentication";
        private const string StrAuthorization = "authorization";

        [ConfigurationProperty(StrAuthentication)]
        public AuthenticationConfig AuthenticationConfig
        {
            get
            {
                return (AuthenticationConfig)this[StrAuthentication];
            }
        }

        [ConfigurationProperty(StrAuthorization)]
        public AuthorizationConfig AuthorizationConfig
        {
            get
            {
                return (AuthorizationConfig)this[StrAuthorization];
            }
        }
    }
}
