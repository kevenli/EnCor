using System;
using EnCor.ObjectBuilder;

namespace EnCor.Security.AuthenticationAdapters
{
    public class AspnetAuthenticationAdapterConfig : AuthenticationAdapterConfig, IAssembler<IAuthenticationAdapter, AuthenticationAdapterConfig>
    {
        #region IAssembler<IAuthenticationAdapter,AuthenticationAdapterConfig> Members
        public IAuthenticationAdapter Assemble(IBuilderContext context, AuthenticationAdapterConfig objectConfiguration)
        {
            var config = objectConfiguration as AspnetAuthenticationAdapterConfig;
            if (config == null)
            {
                throw new Exception(string.Format("Configuration instance {0} is not AspnetAuthenticationAdapterConfig", objectConfiguration));
            }
            return new AspnetAuthenticationAdapter();
        }

        #endregion
    }
}
