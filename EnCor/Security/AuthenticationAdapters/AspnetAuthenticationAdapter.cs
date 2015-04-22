using System;
using System.Web;
using System.Web.Security;
using EnCor.ObjectBuilder;

namespace EnCor.Security.AuthenticationAdapters
{
    [AssembleConfig(typeof(AspnetAuthenticationAdapterConfig))]
    public class AspnetAuthenticationAdapter : AuthenticationAdapter
    {
        public override Credential GetCredential()
        {
            HttpContext httpContext = HttpContext.Current;
            if (httpContext == null)
            {
                return null;
            }


            HttpCookie cookie = httpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie == null)
            {
                return null;
            }

            try
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
                return new AspnetCredential(ticket);
            }
            catch (ArgumentException)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public override void SetCredential(AuthenticateToken token)
        {
            if (token == null)
            {
                throw new ArgumentNullException("token");
            }
            var ticket = CreateTicket(token.Identity.Name);
            string ticketString = FormsAuthentication.Encrypt(ticket);
            HttpContext.Current.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, ticketString));
        }

        public override void ClearCredential()
        {
            var httpCookie = HttpContext.Current.Response.Cookies[FormsAuthentication.FormsCookieName];
            if (httpCookie != null)
                httpCookie.Expires = DateTime.Now.AddMonths(-1);
        }

        private static FormsAuthenticationTicket CreateTicket(string userName)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddMinutes(30), true, "");
            return ticket;
        }
    }
}
