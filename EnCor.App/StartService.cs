using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceProcess;
using EnCor;

namespace EnCor.AppRuntime
{
    public class StartService : ServiceBase
    {
        public StartService()
        {
        }
        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
            Runtime.Startup();
        }

        protected override void OnStop()
        {
            Runtime.Stop();
            base.OnStop();
        }
    }
}
