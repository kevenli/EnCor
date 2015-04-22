using System;
using System.Web;
using System.Web.Security;
using EnCor.ObjectBuilder;
using EnCor.Security.AuthenticationAdapters;

namespace EnCor.Security
{
    [AssembleConfig(typeof(AspnetTokenAdapterConfig))]
    public sealed class AspnetTokenAdapter : AuthenticationAdapter
    {
        public AspnetTokenAdapter()
            : this(false)
        {
        }

        public AspnetTokenAdapter(bool readOnly)
        {
            IsReadOnly = readOnly;
        }

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
                return new AspnetTokenCredential(ticket);
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
            if ( token == null)
            {
                throw new ArgumentNullException("token");
            }
            HttpContext context = HttpContext.Current;

            if (context == null)
            {
                throw new NotSupportedException("Cannot access HttpContext in the context.");
            }
            var ticket = CreateTicket(token.Identity.Name, token.Token);
            string ticketString = FormsAuthentication.Encrypt(ticket);
            context.Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, ticketString));
        }

        public override void ClearCredential()
        {
            HttpContext context = HttpContext.Current;
            if (context == null)
            {
                throw new NotSupportedException("Cannot access HttpContext in the context.");
            }
            var httpCookie = context.Response.Cookies[FormsAuthentication.FormsCookieName];
            if (httpCookie != null)
                httpCookie.Expires = DateTime.Now.AddMonths(-1);
        }

        private static FormsAuthenticationTicket CreateTicket(string userName, string token)
        {
            var ticket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddMinutes(30), true, token);
            return ticket;
        }

        [Obsolete]
        public static void SetAuthenticationCookie(string userName, string token)
        {
            var ticket = CreateTicket(userName, token);
            string encrypteTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypteTicket);
            HttpContext.Current.Response.Cookies.Add(cookie);
        }

        [Obsolete]
        public static void LogOff()
        {
            HttpCookie httpCookie = HttpContext.Current.Response.Cookies[FormsAuthentication.FormsCookieName];
            if (httpCookie != null)
                httpCookie.Expires = DateTime.Now.AddMonths(-1);
            //HttpContext.Current.Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
        }
    }
}
