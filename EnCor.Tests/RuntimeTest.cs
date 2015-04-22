using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using EnCor;
using EnCor.Tests.ModuleAlpha;
using EnCor.Configuration;
using EnCor.Logging;
using EnCor.Tests.ModuleBeta;

namespace EnCor.Tests
{
    [TestFixture]
    public class RuntimeTest
    {
        [TestFixtureSetUp()]
        public static void MyClassInitialize()
        {
            Runtime.Startup();
        }

        [TestFixtureTearDown()]
        public static void MyClassCleanup()
        {
            Runtime.Stop();
        }

        [Test]
        public void StartupTest()
        {

        }

        [Test]
        public void AlphaServiceTest()
        {
            IAlphaService target = (IAlphaService)Runtime.GetService("alpha$AlphaService");
            Console.WriteLine(target.Echo("abc"));
        }

        [Test]
        public void AlphaServiceTest2()
        {
            IAlphaService target = Runtime.GetService<IAlphaService>();
            Assert.AreEqual("abc", target.Echo("abc"));
        }

        [Test]
        public void BetaServiceTest()
        {
            BetaService target = Runtime.GetService<BetaService>();
            Assert.IsNotNull(target);
        }

        [Test]
        public void BetaServiceTest2()
        {
            BetaService target = Runtime.GetService<BetaService>();
            Assert.AreEqual("abc", target.Echo("abc"));
        }

        [Test]
        public void LoggingTest()
        {
            ILogging target = Runtime.Logging;
            Assert.IsNotNull(target);
            target.Info("Log testing");
        }
    }
}
