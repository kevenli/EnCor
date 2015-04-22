using System;
using System.Collections.Generic;
using System.Text;

namespace EnCor.Security
{
    public interface IAuthenticationAdapter
    {
        Credential GetCredential();

        void SetCredential(AuthenticateToken token);

        void ClearCredential();
    }
}
