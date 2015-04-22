using System;
using EnCor.ObjectBuilder;
using EnCor.Security.AuthenticationProviders;
using EnCor.Security.Credentials;
using AspnetAuthenticationProviderConfig = EnCor.Security.AspnetAuthenticationProviderConfig;

namespace EnCor.Security
{
    [AssembleConfig(typeof(AspnetAuthenticationProviderConfig))]
    public class AspnetAuthenticationProvider : AuthenticationProvider
    {
        public override ClaimSet Authenticate(Credential credential)
        {
            throw new NotImplementedException();
        }
    }
}
