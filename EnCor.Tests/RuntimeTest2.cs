using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using EnCor.Configuration;
using EnCor.Tests.ModuleAlpha;

namespace EnCor.Tests
{
    [TestFixture]
    public class RuntimeTest2
    {
        [TestFixtureSetUp()]
        public static void MyClassInitialize()
        {
            Runtime.Startup(FileEnCorConfig.GetFileConfig("encor.config"));
        }

        [TestFixtureTearDown()]
        public static void MyClassCleanup()
        {
            Runtime.Stop();
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
            Console.WriteLine(target.Echo("abc"));
        }
    }
}
