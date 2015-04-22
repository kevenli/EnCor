using System;
using System.Web;
using System.Security.Principal;
using EnCor.ObjectBuilder;
using EnCor.Security.AuthenticationAdapters;
using EnCor.Security.Credentials;

namespace EnCor.Security
{
    [AssembleConfig(typeof(AspnetWindowsIntegratedAuthenticationAdapterConfig))]
    public class AspnetWindowsIntegratedAuthenticationAdapter : AuthenticationAdapter
    {
        public override Credential GetCredential()
        {
            if ( HttpContext.Current == null )
            {
                return null;
            }
            HttpRequest request = HttpContext.Current.Request;
            string logonUser = request.ServerVariables["LOGON_USER"];
            if (string.IsNullOrEmpty(logonUser))
            {
                return null;
            }
            var windowsIdentity = new WindowsIdentity(logonUser);
            var credential = new WindowsCredential( windowsIdentity );
            return credential;
        }

        public override void SetCredential(AuthenticateToken token)
        {
            throw new NotImplementedException();
        }

        public override void ClearCredential()
        {
            throw new NotImplementedException();
        }
    }

    [Assembler(typeof(AspnetWindowsIntegratedAuthenticationAdapterAssembler))]
    public class AspnetWindowsIntegratedAuthenticationAdapterConfig : AuthenticationAdapterConfig
    {
    }

    public class AspnetWindowsIntegratedAuthenticationAdapterAssembler : IAssembler<IAuthenticationAdapter, AuthenticationAdapterConfig>
    {

        #region IAssembler<IAuthenticationAdapter,AuthenticationProviderConfig> Members
        public IAuthenticationAdapter Assemble(IBuilderContext context, AuthenticationAdapterConfig objectConfiguration)
        {
            return new AspnetWindowsIntegratedAuthenticationAdapter();
        }

        #endregion
    }
}
