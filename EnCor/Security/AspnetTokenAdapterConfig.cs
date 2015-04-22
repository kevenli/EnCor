using System;
using System.Collections.Generic;
using System.Text;
using EnCor.Security;
using EnCor.ObjectBuilder;
using System.Configuration;

namespace EnCor.Security
{
    [Assembler(typeof(AspnetFormsAuthenticationAdapterAssembler))]
    public class AspnetTokenAdapterConfig : AuthenticationAdapterConfig
    {
    }

    public class AspnetFormsAuthenticationAdapterAssembler : IAssembler<IAuthenticationAdapter, AuthenticationAdapterConfig>
    {

        #region IAssembler<IAuthenticationAdapter,AuthenticationAdapterConfig> Members
        public IAuthenticationAdapter Assemble(IBuilderContext context, AuthenticationAdapterConfig objectConfiguration)
        {
            AspnetTokenAdapterConfig config = objectConfiguration as AspnetTokenAdapterConfig;
            if (config == null)
            {
                throw new Exception(string.Format("Configuration instance {0} is not AspnetTokenAuthenticationAdapterConfig", objectConfiguration));
            }
            return new AspnetTokenAdapter(config.ReadOnly);
        }

        #endregion
    }
}
