using System;
using System.Collections.Generic;
using System.Text;

namespace EnCor.Security
{
    public sealed class SecurityModule : Module, ISecurity
    {
        private readonly IList<IAuthenticationProvider> _authenticationProviders;

        private readonly IList<IAuthenticationAdapter> _authenticationAdapters;

        public SecurityModule(IList<IAuthenticationProvider> authenticationProviders,
        IList<IAuthenticationAdapter> authenticationAdapters )
        {
            _authenticationProviders = authenticationProviders;
            _authenticationAdapters = authenticationAdapters;
        }



        #region ISecurity Members

        public ClaimSet Authenticate(Credential credential)
        {
            List<ClaimObject> claimObjects = new List<ClaimObject>();
            foreach (IAuthenticationProvider authenticationProvider in _authenticationProviders)
            {
                foreach (ClaimObject claim in authenticationProvider.Authenticate(credential))
                {
                    claimObjects.Add(claim);
                }
            }
            return new ClaimSet(claimObjects);
        }

        #endregion

        #region ISecurity Members


        public void InitializeSecurityContext()
        {
            foreach ( var authAdapter in _authenticationAdapters)
            {
                Credential credential = authAdapter.GetCredential();
                if (credential != null)
                {
                    var claimSet = Authenticate(credential);
                    SecurityContext.Current = new SecurityContext(claimSet);
                    return; // use only the first credential;
                }
            }
        }

        #endregion
    }
}
