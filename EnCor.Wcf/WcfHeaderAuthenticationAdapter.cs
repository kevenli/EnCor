using System;
using System.Collections.Generic;
using System.Text;
using EnCor.Security;
using System.ServiceModel;
using System.ServiceModel.Channels;
using EnCor.Security.AuthenticationAdapters;

namespace EnCor.Wcf
{
    public class WcfHeaderAuthenticationAdapter : AuthenticationAdapter
    {
        public const string STR_EnCorWcfAuthenticationHeader = "EnCorWcfAuthenticationHeader";
        public const string STR_Httpencorcodeplexcomwcfsecurity2010 = "http://encor.codeplex.com/wcf/security/2010";


        public override Credential GetCredential()
        {
            OperationContext operationContext = OperationContext.Current;
            if ( operationContext == null )
            {
                return null;
            }

            MessageHeaders headers = operationContext.IncomingMessageHeaders;
            WcfAuthenticationHeader header = headers.GetHeader<WcfAuthenticationHeader>(STR_EnCorWcfAuthenticationHeader, STR_Httpencorcodeplexcomwcfsecurity2010);
            if (header == null)
            {
                return null;
            }

            return new WcfHeaderCredential(header);
                
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
}
