using System.Web.Security;

namespace EnCor.Security
{
    public class AspnetCredential : Credential
    {
        private readonly FormsAuthenticationTicket _ticket;
        public AspnetCredential(FormsAuthenticationTicket ticket)
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
