using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using EnCor.Wcf.Tests.TestModule;
using System.Diagnostics;

namespace EnCor.Wcf.Tests
{
    [TestFixture]
    public class RouterPerformanceTest
    {
        [TestFixtureSetUp]
        public void MyClassInitialize()
        {
            Runtime.Startup();
        }

        [TestFixtureTearDown]
        public void MyClassCleanup()
        {
            Runtime.Stop();
        }

        [Test]
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
