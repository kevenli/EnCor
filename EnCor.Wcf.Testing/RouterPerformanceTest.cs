using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnCor.Wcf.Testing.TestModule;
using System.Diagnostics;

namespace EnCor.Wcf.Testing
{
    [TestClass]
    public class RouterPerformanceTest
    {
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            Runtime.Startup();
        }

        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            Runtime.Stop();
        }

        [TestMethod]
        [DeploymentItem("EnCor.config")]
        public void PerformanceTest()
        {
            int testcount = 10000;
            IEchoService localService = Runtime.GetService<IEchoService>();

            IEchoService remoteService = ClientFactory.CreateClient<IEchoService>("http://localhost:9201/node/EchoService");

            IEchoService thruRouterService = ClientFactory.CreateClient<IEchoService>("net.tcp://localhost:9103/RouterService");

            Stopwatch stopwatcher = new Stopwatch();
            stopwatcher.Start();
            for (int i = 0; i < testcount; i++)
            {
                localService.Echo("Hello");
            }
            stopwatcher.Stop();
            Console.WriteLine("Local {0} calls spend : {1}", testcount, stopwatcher.Elapsed);

            stopwatcher.Start();
            for (int i = 0; i < testcount; i++)
            {
                remoteService.Echo("Hello");
            }
            stopwatcher.Stop();
            Console.WriteLine("Remote {0} calls spend : {1}", testcount, stopwatcher.Elapsed);

            stopwatcher.Start();
            for (int i = 0; i < testcount; i++)
            {
                thruRouterService.Echo("Hello");
            }
            stopwatcher.Stop();
            Console.WriteLine("thruRouterService {0} calls spend : {1}", testcount, stopwatcher.Elapsed);

            (remoteService as IDisposable).Dispose();
            (thruRouterService as IDisposable).Dispose();
        }
    }
}
