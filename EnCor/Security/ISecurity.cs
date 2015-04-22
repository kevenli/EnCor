using System;
using System.Collections.Generic;
using System.Text;
using EnCor.ModuleLoader;

namespace EnCor.Security
{
    public interface ISecurity : IEnCorModule
    {
        ClaimSet Authenticate(Credential credential);

        void InitializeSecurityContext();
    }
}
