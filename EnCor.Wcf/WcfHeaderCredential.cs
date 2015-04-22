using System;
using System.Collections.Generic;
using System.Text;
using EnCor.Security;
using EnCor.Security.Credentials;

namespace EnCor.Wcf
{
    public class WcfHeaderCredential : TokenCredential
    {
        private WcfAuthenticationHeader _Header;
        public WcfHeaderCredential(WcfAuthenticationHeader header)
        { 
            _Header = header;
        } 

        public override string Token
        {
            get { return _Header.Token; }
        }

        public WcfAuthenticationHeader Header
        {
            get { return _Header; }
        }
    }
}
