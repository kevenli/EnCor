using System.Configuration;
using EnCor.Configuration;

namespace EnCor.Security
{
    public class AuthorizationProviderConfig : NameTypeConfigElement
    {
        const string StrClaimType = "claimType";

        [ConfigurationProperty(StrClaimType)]
        public string ClaimType
        {
            get
            {
                return (string)this[StrClaimType];
            }
        }
    }
}
