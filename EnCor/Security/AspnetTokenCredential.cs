using System.Web.Security;

namespace EnCor.Security
{
    public class  AspnetTokenCredential : Credential
    {
        private readonly FormsAuthenticationTicket _ticket;

        public AspnetTokenCredential(FormsAuthenticationTicket ticket)
        {
            _ticket = ticket;
        }

        public FormsAuthenticationTicket Ticket
        {
            get
            {
                return _ticket;
            }
        }
    }
}
