using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;

namespace EnCor.Security.Credentials
{
    public sealed class WindowsCredential : Credential
    {
        private WindowsIdentity _Identity;

        public WindowsCredential(WindowsIdentity identity)
        {
            _Identity = identity;
        }

        public WindowsIdentity Identity
        {
            get
            {
                return _Identity;
            }
        }
    }
}
