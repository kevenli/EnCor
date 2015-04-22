using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Security;
using System.ServiceModel.Channels;

namespace EnCor.Wcf
{
    public class AspnetFormsTokenHeaderBuilder: HeaderBuilder
    {
        public override IList<System.ServiceModel.Channels.MessageHeader> BuildHeaders()
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


            FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
            MessageHeader header = MessageHeader.CreateHeader(WcfHeaderAuthenticationAdapter.STR_EnCorWcfAuthenticationHeader, 
                WcfHeaderAuthenticationAdapter.STR_Httpencorcodeplexcomwcfsecurity2010, 
                ticket.UserData);
            return new List<MessageHeader> { header };
        }


    }
}
