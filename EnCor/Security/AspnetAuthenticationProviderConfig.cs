using System;
using EnCor.ObjectBuilder;

namespace EnCor.Security
{
    public class AspnetAuthenticationProviderConfig : AuthenticationProviderConfig, IAssembler<IAuthenticationProvider, AuthenticationProviderConfig>
    {
        #region IAssembler<IAuthenticationProvider,AuthenticationProviderConfig> Members
        public IAuthenticationProvider Assemble(IBuilderContext context, AuthenticationProviderConfig objectConfiguration)
        {
            var config = objectConfiguration as AspnetAuthenticationProviderConfig;
            if (config == null)
            {
                throw new Exception(string.Format("Invalid config type: {0}", objectConfiguration));
            }

            var provider = new AspnetAuthenticationProvider();
            return provider;
        }

        #endregion
    }
}
