using System;
using System.Web;

namespace EnCor.Web
{
    public class AspnetRuntimeStarter : IHttpModule
    {
        private HttpApplication _application;
        #region IHttpModule Members

        public void Dispose()
        {
            _application.BeginRequest -= Application_BeginRequest;
            _application = null;
        }

        public void Init(HttpApplication context)
        {
            _application = context;
            _application.AuthenticateRequest += new EventHandler(Application_AuthenticateRequest);
            context.BeginRequest += new EventHandler(Application_BeginRequest);
            _application.PostAuthenticateRequest += new EventHandler(_Application_PostAuthenticateRequest);
            Runtime.Startup();
        }

        void _Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
        }

        void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            //Runtime.InitializeSecurityContext();
            //HttpContext.Current.User = SecurityContext.Current.Principal;
        }

        #endregion

        private void Application_BeginRequest(object sender, EventArgs e)
        {
        }
    }
}
