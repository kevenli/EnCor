using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using EnCor.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EnCor.Test.Logging
{
    [TestClass]
    public class LoggingTest
    {
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            Runtime.Startup(FileEnCorConfig.GetFileConfig("logging.config"));
        }


        [ClassCleanup()]
        public static void MyClassCleanup()
        {
            Runtime.Stop();
        }

        [TestMethod]
        [DeploymentItem("logging/logging.config")]
        public void InfoTest()
        {
            Runtime.Logging.Info("ABC");
        }
    }
}
